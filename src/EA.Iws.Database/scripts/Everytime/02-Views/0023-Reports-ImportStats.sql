IF OBJECT_ID('[Reports].[ImportStats]') IS NULL
    EXEC('CREATE VIEW [Reports].[ImportStats] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[ImportStats]
AS

SELECT
    CASE WHEN M.[QuantityReceivedUnitId] IN (1, 2) -- Tonnes / Cubic Metres
        THEN COALESCE(M.[QuantityReceived], 0)
    ELSE 
        COALESCE(M.[QuantityReceived] / 1000, 0) -- Convert to Tonnes / Cubic Metres
    END AS QuantityReceived,
    M.ReceivedDate,
    N.[CompetentAuthority],
    WT.Description AS WasteStreams,
    TR.ExportCountryCode AS [CountryOfExport],
    TS.TransitStates,
    (SELECT TOP 1 WC.Code
        FROM [Reports].[WasteCodes] WC
        WHERE WC.CodeType IN (1, 2) AND WC.NotificationId = M.NotificationId) AS [BaselOecd],
    STUFF(( SELECT ', ' + WC.Code AS [text()]
                FROM [Reports].[WasteCodes] WC
                WHERE WC.NotificationId = M.NotificationId AND WC.CodeType = 3
                order by 1
                FOR XML PATH('')
                ), 1, 1, '' ) AS [EWC],
    STUFF(( SELECT ', ' + WC.Code AS [text()]
                FROM [Reports].[WasteCodes] WC
                WHERE WC.NotificationId = M.NotificationId AND WC.CodeType = 4
                order by 1
                FOR XML PATH('')
                ), 1, 1, '' ) AS [Ycode],
    STUFF(( SELECT ', ' + WC.Code AS [text()]
                FROM [Reports].[WasteCodes] WC
                WHERE WC.NotificationId = M.NotificationId AND WC.CodeType = 5
                order by 1
                FOR XML PATH('')
                ), 1, 1, '' ) AS [Hcode],
    STUFF(( SELECT ', ' + WC.Description AS [text()]
                FROM [Reports].[WasteCodes] WC
                WHERE WC.NotificationId = M.NotificationId AND WC.CodeType = 5
                order by 1
                FOR XML PATH('')
                ), 1, 1, '' ) AS [HcodeDescription],
    STUFF(( SELECT ', ' + WC.Code AS [text()]
                FROM [Reports].[WasteCodes] WC
                WHERE WC.NotificationId = M.NotificationId AND WC.CodeType = 6
                order by 1
                FOR XML PATH('')
                ), 1, 1, '' ) AS [UN],
CASE
    WHEN N.NotificationType = 1 THEN OCC.OperationCodes
    ELSE NULL
END AS [RCode],
CASE
    WHEN N.NotificationType = 2 THEN OCC.OperationCodes
    ELSE NULL
END AS [DCode]
FROM
    [Reports].[Movements] M
    INNER JOIN [ImportNotification].[Notification] N on N.Id = M.NotificationId
    INNER JOIN [Reports].[TransportRoute] TR ON TR.NotificationId = M.NotificationId
    LEFT JOIN [Reports].[TransitStatesConcat] TS ON TS.NotificationId = M.NotificationId
    INNER JOIN [Reports].[OperationCodesConcat] OCC ON OCC.NotificationId = M.NotificationId
    INNER JOIN [Reports].[WasteType] WT ON WT.NotificationId = M.NotificationId
WHERE
    M.Status IN (3, 4)

GO