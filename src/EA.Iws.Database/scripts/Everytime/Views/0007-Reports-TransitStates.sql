IF OBJECT_ID('[Reports].[TransitStates]') IS NULL
    EXEC('CREATE VIEW [Reports].[TransitStates] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[TransitStates]
AS

	SELECT 
		N.Id AS NotificationId,
		REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
		TR.Id AS TransportRouteId,
		C.Id AS CountryId,
		C.Name AS CountryName,
		C.IsoAlpha2Code AS CountryCode
	FROM [Notification].[TransitState] TS
	INNER JOIN [Notification].[TransportRoute] TR ON TS.TransportRouteId = TR.Id
	INNER JOIN [Notification].[Notification] N ON TR.NotificationId = N.Id
	INNER JOIN [Lookup].[Country] C ON TS.CountryId = C.Id

GO