IF OBJECT_ID('[Reports].[OperationCodesConcat]') IS NULL
    EXEC('CREATE VIEW [Reports].[OperationCodesConcat] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[OperationCodesConcat]
AS
    SELECT
        O.NotificationId,
        STUFF(( SELECT ', ' + OC.Name AS [text()]
               FROM [Reports].[OperationCodes] OC
               WHERE OC.NotificationId = O.NotificationId
               ORDER BY OC.OperationCodeId
               FOR XML PATH('')
             ), 1, 1, '' ) AS OperationCodes
    FROM ( SELECT DISTINCT NotificationId FROM [Reports].[OperationCodes] ) O
GO