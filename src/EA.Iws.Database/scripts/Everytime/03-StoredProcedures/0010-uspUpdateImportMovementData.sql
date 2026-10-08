IF OBJECT_ID('[ImportNotification].[uspUpdateImportMovementData]') IS NULL
    EXEC('CREATE PROCEDURE [ImportNotification].[uspUpdateImportMovementData] AS SET NOCOUNT ON;')
GO

/****** Object:  StoredProcedure [ImportNotification].[uspUpdateImportMovementData]    
Script Date: 20/07/2026 15:00:00 
Description: Record shipment data screen allows "Was the shipment accepted?" field to be editable along with all associated fields.
             Each outcome branch refuses to change Movement.Status unless the corresponding child row exists or can be inserted 
             from the supplied parameters. Without these guards the SP could leave movement.Status pointing at an outcome 
             (received/rejected/partiallyrejected) with no backing row, which violates the invariant NotificationMovementsQuantity.Received 
             relies on and crashes the movements summary page. ******/
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

    BEGIN TRY
        BEGIN TRAN

        -- UPDATE MOVEMENT (Common fields only - status change handled separately)
        UPDATE [ImportNotification].[Movement]
        SET [ActualShipmentDate] = ISNULL(@ActualDate, [ActualShipmentDate]) 
           ,[PrenotificationDate] = @PrenotificationDate 
           ,[StatsMarking] = @StatsMarking
           ,[Comments] = @Comments
        WHERE [Id] = @MovementId AND [NotificationId] = @NotificationId

        -- Validate input: only one status should be set
        IF (CAST(@IsReceived AS INT) + CAST(@IsRejected AS INT) + CAST(@IsPartiallyRejected AS INT)) > 1
        BEGIN
            RAISERROR('Only one shipment status can be set at a time', 16, 1)
            ROLLBACK
            RETURN
        END

        -- Handle status changes with guards
        IF @IsReceived = 1
        BEGIN
            -- Guard: Ensure we have the required data to create receipt row if it doesn't exist
            IF @ReceiptDate IS NULL OR @Quantity IS NULL OR @Unit IS NULL
            BEGIN
                -- Check if receipt row already exists
                IF NOT EXISTS(SELECT 1 FROM [ImportNotification].[MovementReceipt] WHERE [MovementId] = @MovementId)
                BEGIN
                    RAISERROR('Cannot set status to Received: Required receipt data (ReceiptDate, Quantity, Unit) is missing and no existing receipt row found', 16, 1)
                    ROLLBACK
                    RETURN
                END
            END

            -- Delete any existing rejection/partial rejection records
            DELETE FROM [ImportNotification].[MovementRejection] WHERE [MovementId] = @MovementId
            DELETE FROM [ImportNotification].[MovementPartialRejection] WHERE [MovementId] = @MovementId

            -- Update or insert receipt
            IF EXISTS(SELECT 1 FROM [ImportNotification].[MovementReceipt] WHERE [MovementId] = @MovementId)
            BEGIN
                UPDATE [ImportNotification].[MovementReceipt]
                SET [Date] = ISNULL(@ReceiptDate, [Date]) 
                   ,[Quantity] = ISNULL(@Quantity, [Quantity])
                   ,[Unit] = ISNULL(@Unit, [Unit])
                WHERE [MovementId] = @MovementId 
            END
            ELSE
            BEGIN
                INSERT INTO [ImportNotification].[MovementReceipt] ([Id], [MovementId], [Date], [Quantity], [Unit])
                VALUES (NEWID(), @MovementId, @ReceiptDate, @Quantity, @Unit)
            END

            -- Update movement status to Received (not cancelled)
            UPDATE [ImportNotification].[Movement]
            SET [IsCancelled] = 0
            WHERE [Id] = @MovementId
        END
        ELSE IF @IsRejected = 1
        BEGIN
            -- Guard: Ensure we have the required data to create rejection row if it doesn't exist
            IF @RejectiontDate IS NULL
            BEGIN
                -- Check if rejection row already exists
                IF NOT EXISTS(SELECT 1 FROM [ImportNotification].[MovementRejection] WHERE [MovementId] = @MovementId)
                BEGIN
                    RAISERROR('Cannot set status to Rejected: Required rejection date is missing and no existing rejection row found', 16, 1)
                    ROLLBACK
                    RETURN
                END
            END

            -- Delete any existing receipt/partial rejection records AND recovery/disposal date
            DELETE FROM [ImportNotification].[MovementReceipt] WHERE [MovementId] = @MovementId
            DELETE FROM [ImportNotification].[MovementPartialRejection] WHERE [MovementId] = @MovementId
            DELETE FROM [ImportNotification].[MovementOperationReceipt] WHERE [MovementId] = @MovementId

            -- Update or insert rejection
            IF EXISTS(SELECT 1 FROM [ImportNotification].[MovementRejection] WHERE [MovementId] = @MovementId)
            BEGIN
                UPDATE [ImportNotification].[MovementRejection]
                SET [Date] = ISNULL(@RejectiontDate, [Date]) 
                   ,[Reason] = ISNULL(@RejectionReason, [Reason])
                   ,[RejectedQuantity] = ISNULL(@RejectedQuantity, [RejectedQuantity])
                   ,[RejectedUnit] = ISNULL(@RejectedUnit, [RejectedUnit])
                WHERE [MovementId] = @MovementId
            END
            ELSE
            BEGIN
                INSERT INTO [ImportNotification].[MovementRejection] ([Id], [MovementId], [Date], [Reason], [RejectedQuantity], [RejectedUnit])
                VALUES (NEWID(), @MovementId, @RejectiontDate, @RejectionReason, @RejectedQuantity, @RejectedUnit)
            END

            -- Update movement status to Rejected (not cancelled)
            UPDATE [ImportNotification].[Movement]
            SET [IsCancelled] = 0
            WHERE [Id] = @MovementId
        END
        ELSE IF @IsPartiallyRejected = 1
        BEGIN
            -- Guard: Ensure we have the required data to create partial rejection row if it doesn't exist
            IF @RejectiontDate IS NULL OR @Quantity IS NULL OR @Unit IS NULL
            BEGIN
                -- Check if partial rejection row already exists
                IF NOT EXISTS(SELECT 1 FROM [ImportNotification].[MovementPartialRejection] WHERE [MovementId] = @MovementId)
                BEGIN
                    RAISERROR('Cannot set status to Partially Rejected: Required data (RejectionDate, Quantity, Unit) is missing and no existing partial rejection row found', 16, 1)
                    ROLLBACK
                    RETURN
                END
            END

            -- Delete any existing receipt/rejection records
            DELETE FROM [ImportNotification].[MovementReceipt] WHERE [MovementId] = @MovementId
            DELETE FROM [ImportNotification].[MovementRejection] WHERE [MovementId] = @MovementId

            -- Update or insert partial rejection
            IF EXISTS(SELECT 1 FROM [ImportNotification].[MovementPartialRejection] WHERE [MovementId] = @MovementId)
            BEGIN
                UPDATE [ImportNotification].[MovementPartialRejection]
                SET [WasteReceivedDate] = ISNULL(@RejectiontDate, [WasteReceivedDate]) 
                   ,[Reason] = ISNULL(@RejectionReason, [Reason])
                   ,[RejectedQuantity] = ISNULL(@RejectedQuantity, [RejectedQuantity])
                   ,[RejectedUnit] = ISNULL(@RejectedUnit, [RejectedUnit])
                   ,[ActualQuantity] = ISNULL(@Quantity, [ActualQuantity])
                   ,[ActualUnit] = ISNULL(@Unit, [ActualUnit])
                   ,[WasteDisposedDate] = @RecoveryDate  -- Changed from ISNULL to allow NULL clearing
                WHERE [MovementId] = @MovementId
            END
            ELSE
            BEGIN
                INSERT INTO [ImportNotification].[MovementPartialRejection] 
                ([Id], [MovementId], [WasteReceivedDate], [Reason], [ActualQuantity], [ActualUnit], [RejectedQuantity], [RejectedUnit], [WasteDisposedDate])
                VALUES (NEWID(), @MovementId, @RejectiontDate, @RejectionReason, @Quantity, @Unit, @RejectedQuantity, @RejectedUnit, @RecoveryDate)
            END

            -- Update movement status to Partially Rejected (not cancelled)
            UPDATE [ImportNotification].[Movement]
            SET [IsCancelled] = 0
            WHERE [Id] = @MovementId
        END
        ELSE
        BEGIN
            -- Original logic for non-status-change updates (maintaining existing status)
            -- Only update existing rows, do not change status
            IF EXISTS(SELECT 1 FROM [ImportNotification].[MovementReceipt] WHERE [MovementId] = @MovementId)
            BEGIN
                UPDATE [ImportNotification].[MovementReceipt]
                SET [Date] = ISNULL(@ReceiptDate, [Date]) 
                   ,[Quantity] = ISNULL(@Quantity, [Quantity])
                   ,[Unit] = ISNULL(@Unit, [Unit])
                WHERE [MovementId] = @MovementId 
            END
            
            IF EXISTS(SELECT 1 FROM [ImportNotification].[MovementRejection] WHERE [MovementId] = @MovementId)
            BEGIN
                UPDATE [ImportNotification].[MovementRejection]
                SET [Date] = ISNULL(@RejectiontDate, [Date]) 
                   ,[Reason] = ISNULL(@RejectionReason, [Reason])
                   ,[RejectedQuantity] = ISNULL(@RejectedQuantity, [RejectedQuantity])
                   ,[RejectedUnit] = ISNULL(@RejectedUnit, [RejectedUnit])
                WHERE [MovementId] = @MovementId
            END

            IF EXISTS(SELECT 1 FROM [ImportNotification].[MovementPartialRejection] WHERE [MovementId] = @MovementId)
            BEGIN
                UPDATE [ImportNotification].[MovementPartialRejection]
                SET [WasteReceivedDate] = ISNULL(@RejectiontDate, [WasteReceivedDate]) 
                   ,[Reason] = ISNULL(@RejectionReason, [Reason])
                   ,[RejectedQuantity] = ISNULL(@RejectedQuantity, [RejectedQuantity])
                   ,[RejectedUnit] = ISNULL(@RejectedUnit, [RejectedUnit])
                   ,[ActualQuantity] = ISNULL(@Quantity, [ActualQuantity])
                   ,[ActualUnit] = ISNULL(@Unit, [ActualUnit])
                   ,[WasteDisposedDate] = ISNULL(@RecoveryDate, [WasteDisposedDate])
                WHERE [MovementId] = @MovementId
            END
        END

        -- UPDATE or INSERT RECOVERY/DISPOSAL DATE for Accepted and Partially Rejected outcomes
        -- For Rejected outcomes, the row is already deleted above
        IF @IsRejected <> 1 OR @IsRejected IS NULL
        BEGIN
            IF EXISTS(SELECT 1 FROM [ImportNotification].[MovementOperationReceipt] WHERE [MovementId] = @MovementId)
            BEGIN
                IF @RecoveryDate IS NOT NULL
                BEGIN
                    UPDATE [ImportNotification].[MovementOperationReceipt]
                    SET [Date] = @RecoveryDate 
                    WHERE [MovementId] = @MovementId 
                END
                ELSE
                BEGIN
                    -- Recovery date is NULL - delete the existing row to clear it
                    DELETE FROM [ImportNotification].[MovementOperationReceipt] WHERE [MovementId] = @MovementId
                END
            END
            ELSE IF @RecoveryDate IS NOT NULL
            BEGIN
                -- Insert new recovery date row if date is provided and row doesn't exist
                INSERT INTO [ImportNotification].[MovementOperationReceipt] ([Id], [MovementId], [Date])
                VALUES (NEWID(), @MovementId, @RecoveryDate)
            END
        END
        
        COMMIT TRAN
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY()
        DECLARE @ErrorState INT = ERROR_STATE()
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
    END CATCH
END
GO