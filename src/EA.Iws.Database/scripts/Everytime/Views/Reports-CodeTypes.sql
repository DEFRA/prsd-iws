IF OBJECT_ID('[Reports].[WasteCodes]') IS NULL
    EXEC('CREATE VIEW [Reports].[WasteCodes] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[WasteCodes]
AS
	SELECT
		N.Id AS NotificationId,
		N.NotificationNumber,
		WCI.CodeType,
		CT.Name AS CodeTypeName,
		WC.Code,
		WC.Description
	FROM [Notification].[WasteCodeInfo] WCI
	INNER JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
	INNER JOIN [Notification].[Notification] N ON WCI.NotificationId = N.Id
	INNER JOIN [Lookup].[CodeType] CT ON WCI.CodeType = CT.Id
GO