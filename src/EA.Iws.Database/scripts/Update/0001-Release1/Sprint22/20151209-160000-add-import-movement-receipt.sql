CREATE TABLE [ImportNotification].[MovementReceipt]
(
	[Id] UNIQUEIDENTIFIER NOT NULL, 
	[MovementId] UNIQUEIDENTIFIER NOT NULL, 
	[Date] DATETIMEOFFSET NOT NULL,
	[Quantity] DECIMAL(18,4) NOT NULL,
	[Unit] INT NOT NULL CONSTRAINT FK_ImportMovementReceipt_ShipmentQuantity FOREIGN KEY REFERENCES [Lookup].[ShipmentQuantityUnit]([Id]),
	[RowVersion] ROWVERSION NOT NULL,
    CONSTRAINT [PK_ImportMovementReceipt] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ImportMovementReceipt_ImportNotification] FOREIGN KEY ([MovementId]) REFERENCES [ImportNotification].[Movement] (Id)
);
GO

CREATE TABLE [ImportNotification].[MovementRejection]
(
	[Id] UNIQUEIDENTIFIER NOT NULL, 
	[MovementId] UNIQUEIDENTIFIER NOT NULL, 
	[Date] DATETIMEOFFSET NOT NULL,
	[Reason] NVARCHAR(2048) NOT NULL,
	[FurtherDetails] NVARCHAR(MAX) NOT NULL,
	[RowVersion] ROWVERSION NOT NULL,
    CONSTRAINT [PK_ImportMovementRejection] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ImportMovementRejection_ImportNotification] FOREIGN KEY ([MovementId]) REFERENCES [ImportNotification].[Movement] (Id)
);
GO

CREATE TABLE [ImportNotification].[MovementOperationReceipt]
(
	[Id] UNIQUEIDENTIFIER NOT NULL, 
	[MovementId] UNIQUEIDENTIFIER NOT NULL, 
	[Date] DATETIMEOFFSET NOT NULL,
	[RowVersion] ROWVERSION NOT NULL,
    CONSTRAINT [PK_ImportMovementOperationReceipt] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ImportMovementOperationReceipt_ImportNotification] FOREIGN KEY ([MovementId]) REFERENCES [ImportNotification].[Movement] (Id)
);
GO