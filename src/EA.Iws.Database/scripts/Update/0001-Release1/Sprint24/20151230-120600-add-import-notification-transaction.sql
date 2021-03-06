CREATE TABLE [Lookup].[PaymentMethod]
(
	[Id] INT CONSTRAINT PK_PaymentMethod PRIMARY KEY NOT NULL,
	[Description] NVARCHAR(16) NOT NULL
);
GO

INSERT INTO [Lookup].[PaymentMethod] ([Id], [Description])
VALUES 
	(0, 'Cheque'), 
	(1, 'BACS / CHAPS'),
	(2, 'Credit Card'),
	(3, 'Postal Order');
GO 


CREATE TABLE [ImportNotification].[Transaction] (
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_ImportNotification_Transaction] PRIMARY KEY CLUSTERED ([Id] ASC),
	[Date] DATE NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [FK_Transaction_ImportNotification] FOREIGN KEY REFERENCES [ImportNotification].[Notification]([Id]),
	[Debit] DECIMAL (12, 2) NULL,
	[Credit] DECIMAL (12, 2) NULL,
	[PaymentMethod] INT NULL CONSTRAINT [FK_ImportTransaction_PaymentMethod] FOREIGN KEY REFERENCES [Lookup].[PaymentMethod]([Id]),
	[ReceiptNumber] NVARCHAR (100) NULL,
	[Comments] NVARCHAR (500) NULL,
	[RowVersion] ROWVERSION NOT NULL	
);

GO