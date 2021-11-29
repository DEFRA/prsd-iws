INSERT INTO [Lookup].[MovementStatus] ([Id], [Status]) VALUES(8, 'PartiallyRejected')

ALTER TABLE [Notification].[MovementRejection] ADD [RejectedQuantity] decimal(18,4) NULL;
ALTER TABLE [Notification].[MovementRejection] ADD [RejectedUnit] INT NULL;
GO

CREATE TABLE [Notification].[MovementPartialRejection](
	[Id] [uniqueidentifier] NOT NULL,
	[MovementId] [uniqueidentifier] NOT NULL,
	[ActualQuantity] [decimal](18, 4) NULL,
	[ActualUnit] [int] NULL,
	[RejectedQuantity] [decimal](18, 4) NULL,
	[RejectedUnit] [int] NULL,
	[Date] [date] NOT NULL,
	[Reason] [nvarchar](max) NOT NULL,
	[FileId] [uniqueidentifier] NULL,
	[RowVersion] [timestamp] NOT NULL,	
 CONSTRAINT [PK_MovementPartialRejection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [Notification].[MovementPartialRejection]  WITH CHECK ADD  CONSTRAINT [FK_MovementPartialRejection_Movement] FOREIGN KEY([MovementId])
REFERENCES [Notification].[Movement] ([Id])
GO

ALTER TABLE [Notification].[MovementPartialRejection] CHECK CONSTRAINT [FK_MovementPartialRejection_Movement]
GO

ALTER TABLE [ImportNotification].[MovementRejection] ADD [RejectedQuantity] decimal(18,4) NOT NULL;
ALTER TABLE [ImportNotification].[MovementRejection] ADD [RejectedUnit] INT NOT NULL;
GO

CREATE TABLE [ImportNotification].[MovementPartialRejection](
	[Id] [uniqueidentifier] NOT NULL,
	[MovementId] [uniqueidentifier] NOT NULL,
	[ActualQuantity] [decimal](18, 4) NULL,
	[ActualUnit] [int] NULL,
	[RejectedQuantity] [decimal](18, 4) NULL,
	[RejectedUnit] [int] NULL,
	[Date] [date] NOT NULL,
	[Reason] [nvarchar](max) NOT NULL,
	[FileId] [uniqueidentifier] NULL,
	[RowVersion] [timestamp] NOT NULL,	
 CONSTRAINT [PK_MovementPartialRejection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

GO

/****** Object:  StoredProcedure [Notification].[uspUpdateExportMovementData]    Script Date: 25/11/2021 14:11:07 ******/
DROP PROCEDURE [Notification].[uspUpdateExportMovementData]
GO

/****** Object:  StoredProcedure [Notification].[uspUpdateExportMovementData]    Script Date: 25/11/2021 14:11:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Notification].[uspUpdateExportMovementData] 
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
		SET    [Date] = ISNULL(@RejectiontDate, [Date]) 
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


GO

/****** Object:  StoredProcedure [ImportNotification].[uspUpdateImportMovementData]    Script Date: 25/11/2021 14:12:31 ******/
DROP PROCEDURE [ImportNotification].[uspUpdateImportMovementData]
GO

/****** Object:  StoredProcedure [ImportNotification].[uspUpdateImportMovementData]    Script Date: 25/11/2021 14:12:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ImportNotification].[uspUpdateImportMovementData] 
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
		SET    [Date] = ISNULL(@RejectiontDate, [Date]) 
			  ,[Reason] = ISNULL(@RejectionReason, [Reason])
			  ,[RejectedQuantity] = @RejectedQuantity
			  ,[RejectedUnit] = @RejectedUnit
			  ,[ActualQuantity] = @Quantity
			  ,[ActualUnit] = @Unit
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

