IF OBJECT_ID('[Notification].[uspUpdateExportMovementData]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspUpdateExportMovementData] AS SET NOCOUNT ON;')
GO

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
				AS
BEGIN
    SET NOCOUNT ON;

	BEGIN TRAN
	--UPDATE MOVEMENT
	UPDATE [Notification].[Movement]
	SET [Date] = ISNULL(@ActualDate, [Date]) 
	,[PrenotificationDate] = @PrenotificationDate 
	,[HasNoPrenotification] = ISNULL(@HasNoPrenotification, [HasNoPrenotification]) 
	,[StatsMarking] = @StatsMarking 
	,[Comments] = @Comments 
	WHERE [Id]= @MovementId AND [NotificationId] = @NotificationId
		
	--UPDATE RECEIPT
	IF EXISTS(SELECT * FROM [Notification].[MovementReceipt] WHERE [MovementId]= @MovementId)
	BEGIN
		UPDATE [Notification].[MovementReceipt]
		SET [Date] = ISNULL(@ReceiptDate, [Date]) 
		,[Quantity] = ISNULL(@Quantity, [Quantity])
		,[Unit] = ISNULL(@Unit, [Unit])
		WHERE [MovementId]= @MovementId 
	END
	
	IF EXISTS(SELECT * FROM [Notification].[MovementRejection] WHERE [MovementId]= @MovementId)
	BEGIN
		UPDATE [Notification].[MovementRejection]
		SET    [Date] = ISNULL(@RejectiontDate, [Date]) 
			  ,[Reason] = ISNULL(@RejectionReason, [Reason])
			  ,[RejectedQuantity] = @RejectedQuantity
			  ,[RejectedUnit] = @RejectedUnit
		 WHERE [MovementId] = @MovementId
	END

	IF EXISTS(SELECT * FROM [Notification].[MovementPartialRejection] WHERE [MovementId]= @MovementId)
	BEGIN
		UPDATE [Notification].[MovementPartialRejection]
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
	IF EXISTS(SELECT * FROM [Notification].[MovementOperationReceipt] WHERE [MovementId]= @MovementId)
	BEGIN
	IF(@RecoveryDate IS NOT NULL)
		BEGIN
			UPDATE [Notification].[MovementOperationReceipt]
			 SET 
			  [Date] = @RecoveryDate 
			WHERE [MovementId]= @MovementId 
		END
	END
	
	COMMIT;
		
END

GO
