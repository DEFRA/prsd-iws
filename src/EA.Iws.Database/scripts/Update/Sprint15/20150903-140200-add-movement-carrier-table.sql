CREATE TABLE [Notification].[MovementCarrier]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[MovementId] UNIQUEIDENTIFIER NOT NULL,
	[CarrierId] UNIQUEIDENTIFIER NOT NULL,
	[Order] INT NOT NULL,
	[RowVersion] ROWVERSION NOT NULL,
    CONSTRAINT [PK_MovementCarrier] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MovementCarrier_Movement] FOREIGN KEY ([MovementId]) REFERENCES [Notification].[Movement] (Id),
	CONSTRAINT [FK_MovementCarrier_Carrier] FOREIGN KEY ([CarrierId]) REFERENCES [Notification].[Carrier] (Id)
)