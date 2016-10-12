CREATE TABLE [ImportNotification].[WasteType] (
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT PK_ImportNotificationWasteType PRIMARY KEY,
	[ImportNotificationId] [uniqueidentifier] NOT NULL 
		CONSTRAINT FK_ImportNotificationWasteType_ImportNotification FOREIGN KEY REFERENCES [ImportNotification].[Notification]([Id]),
	[Name] [nvarchar](256) NOT NULL,
	[BaselOecdCodeNotListed] [bit] NOT NULL,
	[YCodeNotApplicable] [bit] NOT NULL,
	[HCodeNotApplicable] [bit] NOT NULL,
	[UnClassNotApplicable] [bit] NOT NULL,
	[RowVersion] [timestamp] NOT NULL
);