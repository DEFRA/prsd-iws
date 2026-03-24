IF OBJECT_ID('[ImportNotification].[uspUpdateImportMovementData]') IS NULL
    EXEC('CREATE PROCEDURE [ImportNotification].[uspUpdateImportMovementData] AS SET NOCOUNT ON;')
GO

/****** Object:  StoredProcedure [ImportNotification].[uspUpdateImportMovementData]    
Script Date: 23/03/2026 15:00:00 
Description: Record shipment data screen allow Was the shipment accepted? field editable and all associated fields ******/
ALTER PROCEDURE [ImportNotification].[uspUpdateImportMovementData] 
                @NotificationId UNIQUEIDENTIFIER
                ,@MovementId UNIQUEIDENTIFIER
                ,@PrenotificationDate DATE
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

    --UPDATE MOVEMENT
    UPDATE [ImportNotification].[Movement]
    SET [ActualShipmentDate] = ISNULL(@ActualDate, [ActualShipmentDate]) 
    ,[PrenotificationDate] = @PrenotificationDate 
    ,[StatsMarking] = @StatsMarking
    ,[Comments] = @Comments
    WHERE [Id] = @MovementId AND [NotificationId] = @NotificationId

    -- Handle status changes
    IF @IsReceived = 1
    BEGIN
        -- Delete any existing rejection/partial rejection records
        DELETE FROM [ImportNotification].[MovementRejection] WHERE [MovementId] = @MovementId
        DELETE FROM [ImportNotification].[MovementPartialRejection] WHERE [MovementId] = @MovementId

        -- Update or insert receipt
        IF EXISTS(SELECT * FROM [ImportNotification].[MovementReceipt] WHERE [MovementId] = @MovementId)
        BEGIN
            UPDATE [ImportNotification].[MovementReceipt]
            SET [Date] = ISNULL(@ReceiptDate, [Date]) 
            ,[Quantity] = ISNULL(@Quantity, [Quantity])
            ,[Unit] = ISNULL(@Unit, [Unit])
            WHERE [MovementId] = @MovementId 
        END
        ELSE IF @ReceiptDate IS NOT NULL AND @Quantity IS NOT NULL AND @Unit IS NOT NULL
        BEGIN
            INSERT INTO [ImportNotification].[MovementReceipt] ([Id], [MovementId], [Date], [Quantity], [Unit])
            VALUES (NEWID(), @MovementId, @ReceiptDate, @Quantity, @Unit)
        END

        -- Update movement status to Received
        UPDATE [ImportNotification].[Movement]
        SET [IsCancelled] = 0
        WHERE [Id] = @MovementId
    END
    ELSE IF @IsRejected = 1
    BEGIN
        -- Delete any existing receipt/partial rejection records
        DELETE FROM [ImportNotification].[MovementReceipt] WHERE [MovementId] = @MovementId
        DELETE FROM [ImportNotification].[MovementPartialRejection] WHERE [MovementId] = @MovementId

        -- Update or insert rejection
        IF EXISTS(SELECT * FROM [ImportNotification].[MovementRejection] WHERE [MovementId] = @MovementId)
        BEGIN
            UPDATE [ImportNotification].[MovementRejection]
            SET [Date] = ISNULL(@RejectiontDate, [Date]) 
                ,[Reason] = ISNULL(@RejectionReason, [Reason])
                ,[RejectedQuantity] = @RejectedQuantity
                ,[RejectedUnit] = @RejectedUnit
            WHERE [MovementId] = @MovementId
        END
        ELSE IF @RejectiontDate IS NOT NULL
        BEGIN
            INSERT INTO [ImportNotification].[MovementRejection] ([Id], [MovementId], [Date], [Reason], [RejectedQuantity], [RejectedUnit])
            VALUES (NEWID(), @MovementId, @RejectiontDate, @RejectionReason, @RejectedQuantity, @RejectedUnit)
        END

        -- Update movement
        UPDATE [ImportNotification].[Movement]
        SET [IsCancelled] = 0
        WHERE [Id] = @MovementId
    END
    ELSE IF @IsPartiallyRejected = 1
    BEGIN
        -- Delete any existing receipt/rejection records
        DELETE FROM [ImportNotification].[MovementReceipt] WHERE [MovementId] = @MovementId
        DELETE FROM [ImportNotification].[MovementRejection] WHERE [MovementId] = @MovementId

        -- Update or insert partial rejection
        IF EXISTS(SELECT * FROM [ImportNotification].[MovementPartialRejection] WHERE [MovementId] = @MovementId)
        BEGIN
            UPDATE [ImportNotification].[MovementPartialRejection]
            SET [WasteReceivedDate] = ISNULL(@RejectiontDate, [WasteReceivedDate]) 
                ,[Reason] = ISNULL(@RejectionReason, [Reason])
                ,[RejectedQuantity] = @RejectedQuantity
                ,[RejectedUnit] = @RejectedUnit
                ,[ActualQuantity] = @Quantity
                ,[ActualUnit] = @Unit
                ,[WasteDisposedDate] = @RecoveryDate
            WHERE [MovementId] = @MovementId
        END
        ELSE IF @RejectiontDate IS NOT NULL
        BEGIN
            INSERT INTO [ImportNotification].[MovementPartialRejection] ([Id], [MovementId], [WasteReceivedDate], [Reason], [ActualQuantity], [ActualUnit], [RejectedQuantity], [RejectedUnit], [WasteDisposedDate])
            VALUES (NEWID(), @MovementId, @RejectiontDate, @RejectionReason, @Quantity, @Unit, @RejectedQuantity, @RejectedUnit, @RecoveryDate)
        END

        -- Update movement
        UPDATE [ImportNotification].[Movement]
        SET [IsCancelled] = 0
        WHERE [Id] = @MovementId
    END
    ELSE
    BEGIN
        -- Original logic for non-status-change updates
        IF EXISTS(SELECT * FROM [ImportNotification].[MovementReceipt] WHERE [MovementId] = @MovementId)
        BEGIN
            UPDATE [ImportNotification].[MovementReceipt]
            SET [Date] = ISNULL(@ReceiptDate, [Date]) 
            ,[Quantity] = ISNULL(@Quantity, [Quantity])
            ,[Unit] = ISNULL(@Unit, [Unit])
            WHERE [MovementId] = @MovementId 
        END
        
        IF EXISTS(SELECT * FROM [ImportNotification].[MovementRejection] WHERE [MovementId] = @MovementId)
        BEGIN
            UPDATE [ImportNotification].[MovementRejection]
            SET [Date] = ISNULL(@RejectiontDate, [Date]) 
                ,[Reason] = ISNULL(@RejectionReason, [Reason])
                ,[RejectedQuantity] = @RejectedQuantity
                ,[RejectedUnit] = @RejectedUnit
            WHERE [MovementId] = @MovementId
        END

        IF EXISTS(SELECT * FROM [ImportNotification].[MovementPartialRejection] WHERE [MovementId] = @MovementId)
        BEGIN
            UPDATE [ImportNotification].[MovementPartialRejection]
            SET [WasteReceivedDate] = ISNULL(@RejectiontDate, [WasteReceivedDate]) 
                ,[Reason] = ISNULL(@RejectionReason, [Reason])
                ,[RejectedQuantity] = @RejectedQuantity
                ,[RejectedUnit] = @RejectedUnit
                ,[ActualQuantity] = @Quantity
                ,[ActualUnit] = @Unit
                ,[WasteDisposedDate] = @RecoveryDate
            WHERE [MovementId] = @MovementId
        END
    END

    --UPDATE RECOVERY
    IF EXISTS(SELECT * FROM [ImportNotification].[MovementOperationReceipt] WHERE [MovementId] = @MovementId)
    BEGIN
        IF(@RecoveryDate IS NOT NULL)
        BEGIN
            UPDATE [ImportNotification].[MovementOperationReceipt]
            SET [Date] = @RecoveryDate 
            WHERE [MovementId] = @MovementId 
        END
    END
    
    COMMIT;
END
GO