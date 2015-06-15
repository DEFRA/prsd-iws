DELETE FROM [Business].[ShipmentInfo]
WHERE Units IS NULL
OR Units = 0
GO

ALTER TABLE [Business].[ShipmentInfo]
ALTER COLUMN NumberOfShipments INT NOT NULL
GO
  
ALTER TABLE [Business].[ShipmentInfo]
ALTER COLUMN Quantity DECIMAL(18,4) NOT NULL
GO

ALTER TABLE [Business].[ShipmentInfo]
ALTER COLUMN Units INT NOT NULL
GO

ALTER TABLE [Business].[ShipmentInfo]
ALTER COLUMN FirstDate DATE NOT NULL
GO

ALTER TABLE [Business].[ShipmentInfo]
ALTER COLUMN LastDate DATE NOT NULL
GO