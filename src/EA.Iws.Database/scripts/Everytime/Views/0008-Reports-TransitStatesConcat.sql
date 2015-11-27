IF OBJECT_ID('[Reports].[TransitStatesConcat]') IS NULL
    EXEC('CREATE VIEW [Reports].[TransitStatesConcat] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[TransitStatesConcat]
AS
	SELECT
		N.Id AS NotificationId,
		STUFF(( SELECT ', ' + TS.CountryCode AS [text()]
               FROM [Reports].[TransitStates] TS
               WHERE TS.NotificationId = N.Id
               order by 1
               FOR XML PATH('')
             ), 1, 1, '' ) AS [TransitStates]
	FROM [Notification].[Notification] N
	INNER JOIN ( SELECT DISTINCT NotificationId FROM [Reports].[TransitStates] ) TS ON N.Id = TS.NotificationId
GO