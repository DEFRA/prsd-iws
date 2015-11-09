GO
CREATE TABLE [Notification].[Transaction] (
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Date] DATE NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL,
	[Debit] DECIMAL (12, 2) NULL,
	[Credit] DECIMAL (12, 2) NULL,
	[PaymentMethod] INT NULL,
	[ReceiptNumber] NVARCHAR (100) NULL,
	[Comments] NVARCHAR (500) NULL,
	[RowVersion] ROWVERSION NOT NULL,
	CONSTRAINT [PK_Notification_Transaction] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO