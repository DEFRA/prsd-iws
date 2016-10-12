-- Script updates old data to fix an issue where the Number of Shipments was set to 0

-- Find the occurrence of "TotalShipments": and then add 17 (the length of that search term).
-- This is the index of the number of total shipments.
-- Then find the first occurrence of comma and take all letters between the start of the number and the comma.

WITH Numbers (NumberOfShipments, NotificationId)
AS
	(
	SELECT		SUBSTRING
				(
					SUBSTRING
					(
						Value, 
						CHARINDEX('"TotalShipments":', Value) + 17, 
						1000
					), 
					1, 
					CHARINDEX(',', SUBSTRING(Value, CHARINDEX('"TotalShipments"', Value) + 17, 1000)) - 1
				),
				ImportNotificationId

	FROM		Draft.[Import]

	WHERE		Type = 'EA.Iws.Core.ImportNotification.Draft.Shipment'
	AND			ImportNotificationId	IN 
	(
		SELECT ImportNotificationId 
		FROM [ImportNotification].[Shipment] 
		WHERE NumberOfShipments = 0
	)
)
UPDATE		S
SET			S.NumberOfShipments = N.NumberOfShipments
FROM		[ImportNotification].[Shipment] AS S
INNER JOIN	[Numbers] AS N
ON			N.NotificationId = S.ImportNotificationId;
GO

