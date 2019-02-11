ALTER TABLE [Notification].[MovementCarrier] DROP CONSTRAINT [FK_MovementCarrier_MovementDetails];
GO

ALTER TABLE [Notification].[MovementCarrier]
ALTER COLUMN MovementDetailsId UNIQUEIDENTIFIER NULL;
GO

ALTER TABLE [Notification].[MovementCarrier]
ADD [MovementId] UNIQUEIDENTIFIER NOT NULL;
GO

ALTER TABLE [Notification].[MovementCarrier]
ADD [CreatedOnDate] datetime2 NOT NULL;
GO

ALTER TABLE [Notification].[MovementCarrier]
ADD CONSTRAINT FK_MovementCarrier_Movement 
	FOREIGN KEY ([MovementId]) REFERENCES [Notification].[Movement]([Id]);
GO

