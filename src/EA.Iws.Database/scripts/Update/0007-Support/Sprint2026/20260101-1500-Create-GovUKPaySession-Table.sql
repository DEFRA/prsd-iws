GO
CREATE TABLE [Notification].[GovUkPaySession] (
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL,
	[PaymentId] NVARCHAR (100) NOT NULL,
	[PaymentReference] NVARCHAR (100) NOT NULL,
	[SecureToken] NVARCHAR (100) NOT NULL,
	[Amount] DECIMAL (12, 2) NOT NULL,
	[Status] NVARCHAR (50) NOT NULL,
	[InFinalState] BIT NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[UpdatedDate] DATETIME NULL,
	[RowVersion] ROWVERSION NOT NULL,
	CONSTRAINT [PK_Notification_GovUkPaySession] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_GovUkPaySession_Notification] FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[Notification]([Id])
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GovUkPaySession_SecureToken] ON [Notification].[GovUkPaySession] ([SecureToken] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_GovUkPaySession_NotificationId] ON [Notification].[GovUkPaySession] ([NotificationId] ASC);

GO