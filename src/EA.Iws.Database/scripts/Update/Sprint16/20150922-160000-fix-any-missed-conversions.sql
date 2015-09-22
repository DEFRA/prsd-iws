UPDATE MR
SET MR.Quantity = 
	CASE
		WHEN M.Quantity < MR.Quantity / 50 THEN MR.Quantity / 1000
		WHEN M.Quantity > MR.Quantity * 50 THEN MR.Quantity * 1000
	END
FROM [Notification].[MovementReceipt] AS MR
INNER JOIN [Notification].[Movement] AS M
ON M.Id = MR.MovementId
WHERE MR.Quantity IS NOT NULL
AND 
(
	MR.Quantity * 50 < M.Quantity
	OR MR.Quantity / 50 > M.Quantity
)
GO