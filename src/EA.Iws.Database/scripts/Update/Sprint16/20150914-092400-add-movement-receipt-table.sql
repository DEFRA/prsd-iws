CREATE TABLE [Lookup].[MovementReceiptDecision]
(
	[Id] INT NOT NULL,
	[Name] NVARCHAR(256) NOT NULL,
	CONSTRAINT [PK_MovementReceiptDecision] PRIMARY KEY CLUSTERED ([Id] ASC)
);

INSERT INTO [Lookup].[MovementReceiptDecision] VALUES
(1, 'Accepted'),
(2, 'Rejected');

CREATE TABLE [Notification].[MovementReceipt]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[MovementId] UNIQUEIDENTIFIER NOT NULL,
	[Date] DATETIME NOT NULL,
	[Decision] INT NULL,
	[RejectReason] NVARCHAR(200) NULL,
	[Quantity] DECIMAL(18,4) NULL,
	[RowVersion] ROWVERSION NOT NULL,
	CONSTRAINT [PK_MovementReceipt] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_MovementReceipt_Movement] FOREIGN KEY ([MovementId]) REFERENCES [Notification].[Movement] (Id),
	CONSTRAINT [FK_MovementReceipt_Decision] FOREIGN KEY ([Decision]) REFERENCES [Lookup].[MovementReceiptDecision] (Id)
);