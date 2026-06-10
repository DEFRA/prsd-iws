IF OBJECT_ID('[Notification].[uspUpdateExportMovementData]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspUpdateExportMovementData] AS SET NOCOUNT ON;')
GO

/****** Object:  StoredProcedure [Notification].[uspUpdateExportMovementData]
Script Date: 23/03/2026 12:00:00
Description: Record shipment data screen allow Was the shipment accepted? field editable and all associated fields.
             Each outcome branch refuses to change Movement.Status unless the corresponding child row
             exists or can be inserted from the supplied parameters. Without these guards the SP could
             leave Movement.Status pointing at an outcome (Received / Rejected / PartiallyRejected) with
             no backing row, which violates the invariant NotificationMovementsQuantity.Received relies on
             and crashes the movements summary page. ******/
ALTER PROCEDURE [Notification].[uspUpdateExportMovementData] 
                @NotificationId UNIQUEIDENTIFIER
                ,@MovementId UNIQUEIDENTIFIER
                ,@PrenotificationDate DATE
                ,@HasNoPrenotification bit
                ,@ActualDate DATE
                ,@ReceiptDate DATE
                ,@Quantity DECIMAL(18,4)
                ,@Unit int
                ,@RejectiontDate DATE
                ,@RejectionReason NVARCHAR(MAX)
                ,@StatsMarking NVARCHAR(1024)
                ,@Comments NVARCHAR(MAX)
                ,@RecoveryDate DATE
                ,@CreatedBy nvarchar(128)
                ,@RejectedQuantity DECIMAL(18,4)
                ,@RejectedUnit int
                ,@IsReceived bit = NULL
                ,@IsRejected bit = NULL
                ,@IsPartiallyRejected bit = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRAN

    -- UPDATE MOVEMENT core fields (always). This is the only work done when no outcome flag is set,
    -- i.e. an internal user editing only the actual shipment date on a prenotified movement.
    UPDATE [Notification].[Movement]
    SET [Date]                = ISNULL(@ActualDate, [Date])
       ,[PrenotificationDate] = @PrenotificationDate
       ,[HasNoPrenotification]= ISNULL(@HasNoPrenotification, [HasNoPrenotification])
       ,[StatsMarking]        = @StatsMarking
       ,[Comments]            = @Comments
    WHERE [Id] = @MovementId AND [NotificationId] = @NotificationId

    -- ACCEPTED (Received)
    IF @IsReceived = 1
    BEGIN
        -- Refuse to mark the movement as Received unless we have a receipt to back it up.
        IF NOT EXISTS (SELECT 1 FROM [Notification].[MovementReceipt] WHERE [MovementId] = @MovementId)
           AND (@ReceiptDate IS NULL OR @Quantity IS NULL OR @Unit IS NULL)
        BEGIN
            ROLLBACK TRAN;
            RAISERROR('Cannot mark movement as Received: ReceiptDate, Quantity and Unit are all required when no existing MovementReceipt exists.', 16, 1);
            RETURN;
        END

        -- Remove all rejection/partial rejection/operation receipt data
        DELETE FROM [Notification].[MovementRejection]        WHERE [MovementId] = @MovementId
        DELETE FROM [Notification].[MovementPartialRejection]  WHERE [MovementId] = @MovementId
        DELETE FROM [Notification].[MovementOperationReceipt]  WHERE [MovementId] = @MovementId

        -- Update or insert receipt record
        IF EXISTS(SELECT 1 FROM [Notification].[MovementReceipt] WHERE [MovementId] = @MovementId)
        BEGIN
            UPDATE [Notification].[MovementReceipt]
            SET [Date]     = ISNULL(@ReceiptDate, [Date])
               ,[Quantity] = ISNULL(@Quantity, [Quantity])
               ,[Unit]     = ISNULL(@Unit, [Unit])
            WHERE [MovementId] = @MovementId
        END
        ELSE IF @ReceiptDate IS NOT NULL AND @Quantity IS NOT NULL AND @Unit IS NOT NULL
        BEGIN
            INSERT INTO [Notification].[MovementReceipt] ([Id], [MovementId], [Date], [Quantity], [Unit], [CreatedBy], [CreatedOnDate])
            VALUES (NEWID(), @MovementId, @ReceiptDate, @Quantity, @Unit, @CreatedBy, GETUTCDATE())
        END

        UPDATE [Notification].[Movement]
        SET [Status] = 3 -- Received
        WHERE [Id] = @MovementId
    END

    -- REJECTED
    ELSE IF @IsRejected = 1
    BEGIN
        -- Refuse to mark the movement as Rejected unless we have a rejection to back it up.
        IF NOT EXISTS (SELECT 1 FROM [Notification].[MovementRejection] WHERE [MovementId] = @MovementId)
           AND @RejectiontDate IS NULL
        BEGIN
            ROLLBACK TRAN;
            RAISERROR('Cannot mark movement as Rejected: RejectionDate is required when no existing MovementRejection exists.', 16, 1);
            RETURN;
        END

        -- Remove all receipt/partial rejection/operation receipt data
        DELETE FROM [Notification].[MovementReceipt]           WHERE [MovementId] = @MovementId
        DELETE FROM [Notification].[MovementPartialRejection]  WHERE [MovementId] = @MovementId
        DELETE FROM [Notification].[MovementOperationReceipt]  WHERE [MovementId] = @MovementId

        -- Update or insert rejection record — always overwrite, no ISNULL, to allow full amendments
        IF EXISTS(SELECT 1 FROM [Notification].[MovementRejection] WHERE [MovementId] = @MovementId)
        BEGIN
            UPDATE [Notification].[MovementRejection]
            SET [Date]             = @RejectiontDate
               ,[Reason]           = @RejectionReason
               ,[RejectedQuantity] = @RejectedQuantity
               ,[RejectedUnit]     = @RejectedUnit
            WHERE [MovementId] = @MovementId
        END
        ELSE IF @RejectiontDate IS NOT NULL
        BEGIN
            INSERT INTO [Notification].[MovementRejection] ([Id], [MovementId], [Date], [Reason], [RejectedQuantity], [RejectedUnit])
            VALUES (NEWID(), @MovementId, @RejectiontDate, @RejectionReason, @RejectedQuantity, @RejectedUnit)
        END

        UPDATE [Notification].[Movement]
        SET [Status] = 5 -- Rejected
        WHERE [Id] = @MovementId
    END

    -- PARTIALLY REJECTED
    ELSE IF @IsPartiallyRejected = 1
    BEGIN
        -- Refuse to mark the movement as PartiallyRejected (or Completed, when a recovery date is
        -- supplied) unless we have a partial rejection to back it up.
        IF NOT EXISTS (SELECT 1 FROM [Notification].[MovementPartialRejection] WHERE [MovementId] = @MovementId)
           AND @RejectiontDate IS NULL
        BEGIN
            ROLLBACK TRAN;
            RAISERROR('Cannot mark movement as PartiallyRejected: RejectionDate is required when no existing MovementPartialRejection exists.', 16, 1);
            RETURN;
        END

        -- Remove all receipt/rejection data
        DELETE FROM [Notification].[MovementReceipt]    WHERE [MovementId] = @MovementId
        DELETE FROM [Notification].[MovementRejection]  WHERE [MovementId] = @MovementId

        -- Update or insert partial rejection record — always overwrite, no ISNULL, to allow full amendments
        IF EXISTS(SELECT 1 FROM [Notification].[MovementPartialRejection] WHERE [MovementId] = @MovementId)
        BEGIN
            UPDATE [Notification].[MovementPartialRejection]
            SET [WasteReceivedDate] = @RejectiontDate
               ,[Reason]            = @RejectionReason
               ,[ActualQuantity]    = @Quantity
               ,[ActualUnit]        = @Unit
               ,[RejectedQuantity]  = @RejectedQuantity
               ,[RejectedUnit]      = @RejectedUnit
               ,[WasteDisposedDate] = @RecoveryDate
            WHERE [MovementId] = @MovementId
        END
        ELSE IF @RejectiontDate IS NOT NULL
        BEGIN
            INSERT INTO [Notification].[MovementPartialRejection] ([Id], [MovementId], [WasteReceivedDate], [Reason], [ActualQuantity], [ActualUnit], [RejectedQuantity], [RejectedUnit], [WasteDisposedDate])
            VALUES (NEWID(), @MovementId, @RejectiontDate, @RejectionReason, @Quantity, @Unit, @RejectedQuantity, @RejectedUnit, @RecoveryDate)
        END

        -- Handle recovery date and operation receipt
        IF @RecoveryDate IS NOT NULL
        BEGIN
            IF EXISTS(SELECT 1 FROM [Notification].[MovementOperationReceipt] WHERE [MovementId] = @MovementId)
            BEGIN
                UPDATE [Notification].[MovementOperationReceipt]
                SET [Date] = @RecoveryDate
                WHERE [MovementId] = @MovementId
            END
            ELSE
            BEGIN
                INSERT INTO [Notification].[MovementOperationReceipt] ([Id], [MovementId], [Date], [CreatedBy], [CreatedOnDate])
                VALUES (NEWID(), @MovementId, @RecoveryDate, @CreatedBy, GETUTCDATE())
            END

            UPDATE [Notification].[Movement]
            SET [Status] = 4 -- Completed
            WHERE [Id] = @MovementId
        END
        ELSE
        BEGIN
            -- Recovery date cleared — remove operation receipt and revert to PartiallyRejected
            DELETE FROM [Notification].[MovementOperationReceipt] WHERE [MovementId] = @MovementId

            UPDATE [Notification].[Movement]
            SET [Status] = 8 -- PartiallyRejected
            WHERE [Id] = @MovementId
        END
    END

    COMMIT;
END
GO