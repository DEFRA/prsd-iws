CREATE TABLE [ImportNotification].[WasteOperation](
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_ImportNotificationWasteOperation PRIMARY KEY,
	[ImportNotificationId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_ImportNotificationWasteOperation_ImportNotification
		FOREIGN KEY REFERENCES [ImportNotification].[Notification]([Id]),
	[TechnologyEmployed] NVARCHAR(70) NULL,
	[RowVersion] TIMESTAMP NOT NULL
	)
GO

CREATE TABLE [ImportNotification].[OperationCodes](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT PK_ImportNotificationOperationCodes PRIMARY KEY,
	[WasteOperationId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationOperationCodes_WasteOperation
		FOREIGN KEY REFERENCES [ImportNotification].[WasteOperation]([Id]),
	[OperationCode] [int] NOT NULL CONSTRAINT FK_ImportNotificationOperationCodes_OperationCode
		FOREIGN KEY REFERENCES [Lookup].[OperationCode]([Id]),
	[RowVersion] [timestamp] NOT NULL
	)

GO