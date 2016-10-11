ALTER TABLE [ImportNotification].[Producer]
ADD [IsOnlyProducer] BIT NULL;

GO

UPDATE [ImportNotification].[Producer]
SET [IsOnlyProducer] = 0;

GO

ALTER TABLE [ImportNotification].[Producer]
ALTER COLUMN [IsOnlyProducer] BIT NOT NULL;

GO