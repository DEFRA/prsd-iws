IF OBJECT_ID('[Reports].[TransitStatesConcat]') IS NULL
    EXEC('CREATE VIEW [Reports].[TransitStatesConcat] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[TransitStatesConcat]
AS
    SELECT
        T.NotificationId,
        STUFF(( SELECT ', ' + TS.CountryCode AS [text()]
               FROM [Reports].[TransitStates] TS
               WHERE TS.NotificationId = t.NotificationId
               order by 1
               FOR XML PATH('')
             ), 1, 1, '' ) AS [TransitStates]
    FROM ( SELECT DISTINCT NotificationId FROM [Reports].[TransitStates] ) T
GO