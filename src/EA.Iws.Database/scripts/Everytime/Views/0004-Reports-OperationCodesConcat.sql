IF OBJECT_ID('[Reports].[OperationCodesConcat]') IS NULL
    EXEC('CREATE VIEW [Reports].[OperationCodesConcat] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[OperationCodesConcat]
AS
    SELECT
        N.Id AS NotificationId,
        STUFF(( SELECT ', ' + OC.Name AS [text()]
               FROM [Reports].[OperationCodes] OC
               WHERE OC.NotificationId = N.Id
               ORDER BY OC.OperationCodeId
               FOR XML PATH('')
             ), 1, 1, '' ) AS OperationCodes
    FROM [Reports].[Notification] N
    INNER JOIN ( SELECT DISTINCT NotificationId FROM [Reports].[OperationCodes] ) OC ON N.Id = OC.NotificationId
GO