UPDATE [Notification].[ShipmentInfo]
SET [Quantity] = ROUND([Quantity], 1)
WHERE [Units] IN (3, 4)

UPDATE [Notification].[Movement]
SET [Quantity] = ROUND([Quantity], 1)
WHERE [QuantityUnit] IN (3, 4)

UPDATE MR
SET MR.[Quantity] = ROUND(MR.[Quantity], 1)
FROM [Notification].[MovementReceipt] AS MR
INNER JOIN [Notification].[Movement] AS M
	ON M.[Id] = MR.[MovementId]
WHERE M.[QuantityUnit] IN (3, 4)