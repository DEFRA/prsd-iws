IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Notification].[MovementReceipt]') AND name = 'Unit')
BEGIN
	ALTER TABLE [Notification].[MovementReceipt]
	ADD [Unit] INT NULL;
END
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


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_MovementReceipt_ShipmentQuantityUnit')
BEGIN
	ALTER TABLE [Notification].[MovementReceipt]
	ADD CONSTRAINT FK_MovementReceipt_ShipmentQuantityUnit FOREIGN KEY ([Unit]) REFERENCES [Lookup].[ShipmentQuantityUnit]([Id]);
END
GO