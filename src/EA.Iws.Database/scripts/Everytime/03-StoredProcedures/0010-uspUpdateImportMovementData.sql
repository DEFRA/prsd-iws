IF OBJECT_ID('[ImportNotification].[uspUpdateImportMovementData]') IS NULL
    EXEC('CREATE PROCEDURE [ImportNotification].[uspUpdateImportMovementData] AS SET NOCOUNT ON;')
GO

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
	WHERE [Id]= @MovementId AND [NotificationId] = @NotificationId
		
	--UPDATE RECEIPT
	IF EXISTS(SELECT * FROM [ImportNotification].[MovementReceipt] WHERE [MovementId]= @MovementId)
	BEGIN
		UPDATE [ImportNotification].[MovementReceipt]
		SET [Date] = ISNULL(@ReceiptDate, [Date]) 
		,[Quantity] = ISNULL(@Quantity, [Quantity])
		,[Unit] = ISNULL(@Unit, [Unit])
		WHERE [MovementId]= @MovementId 
	END
	
	IF EXISTS(SELECT * FROM [ImportNotification].[MovementRejection] WHERE [MovementId]= @MovementId)
	BEGIN
		UPDATE [ImportNotification].[MovementRejection]
		SET    [Date] = ISNULL(@RejectiontDate, [Date]) 
			  ,[Reason] = ISNULL(@RejectionReason, [Reason])
			  ,[RejectedQuantity] = @RejectedQuantity
			  ,[RejectedUnit] = @RejectedUnit
		 WHERE [MovementId] = @MovementId

	END

	IF EXISTS(SELECT * FROM [ImportNotification].[MovementPartialRejection] WHERE [MovementId]= @MovementId)
	BEGIN
		UPDATE [ImportNotification].[MovementPartialRejection]
		SET    [WasteReceivedDate] = ISNULL(@RejectiontDate, [WasteReceivedDate]) 
			  ,[Reason] = ISNULL(@RejectionReason, [Reason])
			  ,[RejectedQuantity] = @RejectedQuantity
			  ,[RejectedUnit] = @RejectedUnit
			  ,[ActualQuantity] = @Quantity
			  ,[ActualUnit] = @Unit
			  ,[WasteDisposedDate] = @RecoveryDate
		 WHERE [MovementId] = @MovementId

	END

	--UPDATE RECOVERY
	IF EXISTS(SELECT * FROM [ImportNotification].[MovementOperationReceipt] WHERE [MovementId]= @MovementId)
	BEGIN
		IF(@RecoveryDate IS NOT NULL)
		BEGIN
			UPDATE [ImportNotification].[MovementOperationReceipt]
			 SET 
			  [Date] = @RecoveryDate 
			WHERE [MovementId]= @MovementId 
		END
	END
	
	COMMIT;
		
END

GO
