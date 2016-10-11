CREATE TABLE [Notification].[UserHistory] (
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL,
	[CurrentUserId] NVARCHAR(128) NOT NULL,
	[NewUserId] NVARCHAR(128) NOT NULL,
	[DateChanged] DATETIME NOT NULL,
	[RowVersion] ROWVERSION NOT NULL,
	CONSTRAINT [PK_UserHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_UserHistory_Notification] FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[Notification] ([Id]),
	CONSTRAINT [FK_UserHistory_CurrentUser] FOREIGN KEY ([CurrentUserId]) REFERENCES [Identity].[AspNetUsers] ([Id]),
	CONSTRAINT [FK_UserHistory_NewUser] FOREIGN KEY ([NewUserId]) REFERENCES [Identity].[AspNetUsers] ([Id])
);
GO