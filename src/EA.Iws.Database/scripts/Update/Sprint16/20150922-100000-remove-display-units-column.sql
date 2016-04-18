UPDATE MR
SET MR.Quantity = 
	CASE 
		WHEN M.DisplayUnit IN (3, 4) THEN ROUND(MR.Quantity * 1000, 4)
		WHEN M.DisplayUnit IN (1, 2) THEN ROUND(MR.Quantity / 1000, 4)
	END
FROM [Notification].[MovementReceipt] AS MR
INNER JOIN [Notification].[Movement] AS M
ON M.Id = MR.MovementId
WHERE M.DisplayUnit != M.QuantityUnit

UPDATE [Notification].[Movement]
SET Quantity = 
	CASE 
		WHEN DisplayUnit IN (3, 4) THEN ROUND(Quantity * 1000, 4)
		WHEN DisplayUnit IN (1, 2) THEN ROUND(Quantity / 1000, 4)
	END,
QuantityUnit = DisplayUnit
WHERE DisplayUnit != QuantityUnit
GO

EXEC sp_rename '[Notification].[Movement].[QuantityUnit]', 'Unit', 'COLUMN'
GO

ALTER TABLE [Notification].[Movement]
DROP CONSTRAINT FK_DisplayUnit_ShipmentQuantityUnit
GO

ALTER TABLE [Notification].[Movement]
DROP COLUMN [DisplayUnit]
GO