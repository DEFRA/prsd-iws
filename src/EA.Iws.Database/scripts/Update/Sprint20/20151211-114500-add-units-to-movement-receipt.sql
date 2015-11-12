ALTER TABLE [Notification].[MovementReceipt]
ADD [Unit] INT NULL;
GO

UPDATE MR
SET MR.[Unit] = MD.[Unit]
FROM
	[Notification].[MovementReceipt] MR
	INNER JOIN [Notification].[MovementDetails] MD
		ON MR.[MovementId] = MD.[MovementId];
GO

ALTER TABLE [Notification].[MovementReceipt]
ALTER COLUMN [Unit] INT NOT NULL;
GO

ALTER TABLE [Notification].[MovementReceipt]
ADD CONSTRAINT FK_MovementReceipt_ShipmentQuantityUnit FOREIGN KEY ([Unit]) REFERENCES [Lookup].[ShipmentQuantityUnit]([Id]);
GO