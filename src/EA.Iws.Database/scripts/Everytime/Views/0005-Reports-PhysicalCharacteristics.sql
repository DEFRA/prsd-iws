IF OBJECT_ID('[Reports].[PhysicalCharacteristics]') IS NULL
    EXEC('CREATE VIEW [Reports].[PhysicalCharacteristics] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[PhysicalCharacteristics]
AS

	SELECT
		N.Id AS NotificationId,
		N.NotificationNumber,
		PC.PhysicalCharacteristicType,
		CASE 
			WHEN PC.PhysicalCharacteristicType = 7 THEN PC.OtherDescription
			ELSE PCT.Description
		END AS Description
	FROM
		[Notification].[Notification] N
		INNER JOIN [Notification].[PhysicalCharacteristicsInfo] PC ON PC.NotificationId = N.Id
		INNER JOIN [Lookup].[PhysicalCharacteristicType] PCT ON PC.PhysicalCharacteristicType = PCT.Id
GO