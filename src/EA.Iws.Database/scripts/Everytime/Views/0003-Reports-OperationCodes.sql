IF OBJECT_ID('[Reports].[OperationCodes]') IS NULL
    EXEC('CREATE VIEW [Reports].[OperationCodes] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[OperationCodes]
AS
    SELECT
        N.Id AS NotificationId,
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        O.Id as OperationCodeId,
        O.Name
    FROM [Notification].[OperationCodes] OC
    INNER JOIN [Notification].[Notification] N ON OC.NotificationId = N.Id
    INNER JOIN [Lookup].[OperationCode] O ON OC.OperationCode = O.Id

    UNION ALL

    SELECT
        N.Id AS NotificationId,
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        O.Id as OperationCodeId,
        O.Name
    FROM [ImportNotification].[WasteOperation] WO
    INNER JOIN [ImportNotification].[OperationCodes] OC ON OC.WasteOperationId = WO.Id
    INNER JOIN [ImportNotification].[Notification] N ON WO.ImportNotificationId = N.Id
    INNER JOIN [Lookup].[OperationCode] O ON OC.OperationCode = O.Id
GO