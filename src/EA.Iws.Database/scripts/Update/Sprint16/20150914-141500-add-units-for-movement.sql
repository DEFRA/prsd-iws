CREATE TABLE [Lookup].[ShipmentQuantityUnit]
(
	[Id] INT CONSTRAINT PK_ShipmentQuantityUnit PRIMARY KEY NOT NULL,
	[Description] NVARCHAR(64) NOT NULL
);
GO

INSERT INTO [Lookup].[ShipmentQuantityUnit] ([Id], [Description])
VALUES	(1, 'Tonnes'),
		(2, 'Cubic Metres'),
		(3, 'Kilograms'),
		(4, 'Litres');
GO

EXEC sp_rename '[Notification].[Movement].[Units]', 'QuantityUnit', 'COLUMN';
GO

ALTER TABLE [Notification].[Movement]
ADD [DisplayUnit] INT NULL CONSTRAINT FK_DisplayUnit_ShipmentQuantityUnit FOREIGN KEY REFERENCES [Lookup].[ShipmentQuantityUnit]([Id])
GO

ALTER TABLE [Notification].[Movement]
ADD CONSTRAINT FK_NotificationUnit_ShipmentQuantityUnit FOREIGN KEY ([QuantityUnit]) REFERENCES [Lookup].[ShipmentQuantityUnit]([Id]);
GO

UPDATE [Notification].[Movement]
SET [DisplayUnit] = [QuantityUnit]
WHERE [DisplayUnit] IS NULL