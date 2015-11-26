IF OBJECT_ID('[Reports].[TransportRoute]') IS NULL
    EXEC('CREATE VIEW [Reports].[TransportRoute] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[TransportRoute]
AS

	SELECT 
		N.Id AS NotificationId,
		N.NotificationNumber,
		TR.Id AS TransportRouteId,
		SE_C.Id AS ExportCountryId,
		SE_C.Name AS ExportCountryName,
		SE_C.IsoAlpha2Code AS ExportCountryCode,
		SI_C.Id AS ImportCountryId,
		SI_C.Name AS ImportCountryName,
		SI_C.IsoAlpha2Code AS ImportCountryCode
	FROM [Notification].[TransportRoute] TR
	INNER JOIN [Notification].[Notification] N ON TR.NotificationId = N.Id
	INNER JOIN [Notification].[StateOfExport] SE ON SE.TransportRouteId = TR.Id
	INNER JOIN [Notification].[StateOfImport] SI ON SI.TransportRouteId = TR.Id
	INNER JOIN [Lookup].[Country] SE_C ON SE.CountryId = SE_C.Id
	INNER JOIN [Lookup].[Country] SI_C ON SI.CountryId = SI_C.Id
GO