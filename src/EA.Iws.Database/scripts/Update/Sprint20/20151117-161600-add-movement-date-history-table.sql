CREATE TABLE [Notification].[MovementDateHistory] (
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[MovementId] UNIQUEIDENTIFIER NOT NULL,
	[PreviousDate] DATETIME NOT NULL,
	[DateChanged] DATETIME NOT NULL,
	[RowVersion] ROWVERSION NOT NULL,
	CONSTRAINT [PK_MovementDateHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_MovementDateHistory_Movement] FOREIGN KEY ([MovementId]) REFERENCES [Notification].[Movement] ([Id])
);
GO