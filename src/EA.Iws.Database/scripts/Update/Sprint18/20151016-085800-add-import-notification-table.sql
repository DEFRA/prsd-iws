CREATE TABLE [Lookup].[NotificationType]
(
	[Id] INT CONSTRAINT PK_NotificationType PRIMARY KEY NOT NULL,
	[Description] NVARCHAR(64) NOT NULL
);
GO

INSERT INTO [Lookup].[NotificationType] ([Id], [Description])
VALUES	(1, 'Recovery'),
		(2, 'Disposal');
GO

CREATE TABLE [Notification].[ImportNotification]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT PK_ImportNotification PRIMARY KEY NOT NULL,
	[NotificationNumber] NVARCHAR(50) CONSTRAINT UQ_ImportNotification_NotificationNumber UNIQUE NOT NULL,
	[NotificationType] INT CONSTRAINT FK_ImportNotification_NotificationType FOREIGN KEY REFERENCES [Lookup].[NotificationType]([Id]) NOT NULL,
	[RowVersion] ROWVERSION NOT NULL
);
GO

ALTER TABLE [Notification].[Notification]
ADD CONSTRAINT FK_Notification_NotificationType FOREIGN KEY ([NotificationType]) REFERENCES [Lookup].[NotificationType]([Id]);
GO