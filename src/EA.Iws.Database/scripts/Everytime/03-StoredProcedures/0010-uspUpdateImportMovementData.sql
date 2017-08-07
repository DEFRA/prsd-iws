IF OBJECT_ID('[ImportNotification].[uspUpdateImportMovementData]') IS NULL
    EXEC('CREATE PROCEDURE [ImportNotification].[uspUpdateImportMovementData] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [ImportNotification].[uspUpdateImportMovementData] 
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
                ,@StatsMarking INT
                ,@Comments NVARCHAR(MAX)
				,@RecoveryDate DATE
				,@CreatedBy nvarchar(128)
				AS
BEGIN
    SET NOCOUNT ON;

	BEGIN TRAN
	--UPDATE MOVEMENT
	UPDATE [ImportNotification].[Movement]
	SET [ActualShipmentDate] = ISNULL(@ActualDate, [ActualShipmentDate]) 
	,[PrenotificationDate] = ISNULL(@PrenotificationDate, [PrenotificationDate]) 
	,[StatsMarking] = ISNULL(@StatsMarking, [StatsMarking]) 
	,[Comments] = ISNULL(@Comments, [Comments]) 
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
		 WHERE [MovementId] = @MovementId

	END
	

	--UPDATE RECOVERY
	IF EXISTS(SELECT * FROM [ImportNotification].[MovementOperationReceipt] WHERE [MovementId]= @MovementId)
	BEGIN
		UPDATE [ImportNotification].[MovementOperationReceipt]
		 SET 
		  [Date] = @RecoveryDate 
		WHERE [MovementId]= @MovementId 
	END
	
	COMMIT;
		
END




GO


