IF OBJECT_ID('[Reports].[WasteCodes]') IS NULL
    EXEC('CREATE VIEW [Reports].[WasteCodes] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[WasteCodes]
AS
    SELECT
        N.Id AS NotificationId,
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        WCI.CodeType,
        CT.Name AS CodeTypeName,
        CASE 
            WHEN WCI.CustomCode IS NOT NULL THEN WCI.CustomCode
            ELSE WC.Code 
        END AS [Code],
        WC.Description
    FROM [Notification].[WasteCodeInfo] WCI
    LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
    INNER JOIN [Notification].[Notification] N ON WCI.NotificationId = N.Id
    INNER JOIN [Lookup].[CodeType] CT ON WCI.CodeType = CT.Id

    UNION ALL

    SELECT
        N.Id AS NotificationId,
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        WC.CodeType,
        CT.Name AS CodeTypeName,
        WC.Code,
        WC.Description
    FROM [ImportNotification].[WasteType] WT
    INNER JOIN [ImportNotification].[WasteCode] WCI ON WT.Id = WCI.WasteTypeId
    LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
    INNER JOIN [ImportNotification].[Notification] N ON WT.ImportNotificationId = N.Id
    INNER JOIN [Lookup].[CodeType] CT ON WC.CodeType = CT.Id
GO