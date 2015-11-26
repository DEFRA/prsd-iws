IF OBJECT_ID('[Reports].[PhysicalCharacteristicsConcat]') IS NULL
    EXEC('CREATE VIEW [Reports].[PhysicalCharacteristicsConcat] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[PhysicalCharacteristicsConcat]
AS

	SELECT
		N.Id AS NotificationId,
		STUFF(( SELECT ', ' + PC.Description AS [text()]
               FROM [Reports].[PhysicalCharacteristics] PC
               WHERE PC.NotificationId = N.Id
               ORDER BY PC.PhysicalCharacteristicType
               FOR XML PATH('')
             ), 1, 1, '' ) AS PhysicalCharacteristics
	FROM [Notification].[Notification] N
	INNER JOIN ( SELECT DISTINCT NotificationId FROM [Reports].[PhysicalCharacteristics] ) PC ON N.Id = PC.NotificationId
GO