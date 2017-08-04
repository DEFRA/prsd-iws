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
                ,@StatsMarking INT
                ,@Comments NVARCHAR(MAX)
				,@RecoveryDate DATE
				,@CreatedBy nvarchar(128)
				AS
BEGIN
    SET NOCOUNT ON;

	BEGIN TRAN
	--UPDATE MOVEMENT
	UPDATE [Notification].[Movement]
	SET [Date] = ISNULL(@ActualDate, [Date]) 
	,[PrenotificationDate] = ISNULL(@PrenotificationDate, [PrenotificationDate]) 
	,[HasNoPrenotification] = ISNULL(@HasNoPrenotification, [HasNoPrenotification]) 
	,[StatsMarking] = ISNULL(@StatsMarking, [StatsMarking]) 
	,[Comments] = ISNULL(@Comments, [Comments]) 
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
		 WHERE [MovementId] = @MovementId

	END
	

	--UPDATE RECOVERY
	IF EXISTS(SELECT * FROM [Notification].[MovementOperationReceipt] WHERE [MovementId]= @MovementId)
	BEGIN
		UPDATE [Notification].[MovementOperationReceipt]
		 SET 
		  [Date] = @RecoveryDate 
		WHERE [MovementId]= @MovementId 
	END
	
	COMMIT;
		
END




GO


