IF OBJECT_ID('[Reports].[TransportRoute]') IS NULL
    EXEC('CREATE VIEW [Reports].[TransportRoute] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[TransportRoute]
AS

    SELECT 
        N.Id AS NotificationId,
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        TR.Id AS TransportRouteId,
        SE_C.Id AS ExportCountryId,
        SE_C.Name AS ExportCountryName,
        SE_C.IsoAlpha2Code AS ExportCountryCode,
        SE_E.Id AS ExitPointId,
        SE_E.Name AS ExitPoint,
        SI_C.Id AS ImportCountryId,
        SI_C.Name AS ImportCountryName,
        SI_C.IsoAlpha2Code AS ImportCountryCode,
        SI_E.Id AS EntryPointId,
        SI_E.Name AS EntryPoint
    FROM [Notification].[TransportRoute] TR
    INNER JOIN [Notification].[Notification] N ON TR.NotificationId = N.Id
    INNER JOIN [Notification].[StateOfExport] SE ON SE.TransportRouteId = TR.Id
    INNER JOIN [Notification].[StateOfImport] SI ON SI.TransportRouteId = TR.Id
    INNER JOIN [Notification].[EntryOrExitPoint] SE_E ON SE_E.Id = SE.ExitPointId
    INNER JOIN [Notification].[EntryOrExitPoint] SI_E ON SI_E.Id = SI.EntryPointId
    INNER JOIN [Lookup].[Country] SE_C ON SE.CountryId = SE_C.Id
    INNER JOIN [Lookup].[Country] SI_C ON SI.CountryId = SI_C.Id

    UNION ALL

    SELECT
        N.Id AS NotificationId,
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        TR.Id AS TransportRouteId,
        SE_C.Id AS ExportCountryId,
        SE_C.Name AS ExportCountryName,
        SE_C.IsoAlpha2Code AS ExportCountryCode,
        SE_E.Id AS ExitPointId,
        SE_E.Name AS ExitPoint,
        SI_C.Id AS ImportCountryId,
        SI_C.Name AS ImportCountryName,
        SI_C.IsoAlpha2Code AS ImportCountryCode,
        SI_E.Id AS EntryPointId,
        SI_E.Name AS EntryPoint
    FROM [ImportNotification].[TransportRoute] TR
    INNER JOIN [ImportNotification].[Notification] N ON TR.ImportNotificationId = N.Id
    INNER JOIN [ImportNotification].[StateOfExport] SE ON SE.TransportRouteId = TR.Id
    INNER JOIN [ImportNotification].[StateOfImport] SI ON SI.TransportRouteId = TR.Id
    INNER JOIN [Notification].[EntryOrExitPoint] SE_E ON SE_E.Id = SE.ExitPointId
    INNER JOIN [Notification].[EntryOrExitPoint] SI_E ON SI_E.Id = SI.EntryPointId
    INNER JOIN [Lookup].[Country] SE_C ON SE.CountryId = SE_C.Id
    INNER JOIN (
        SELECT TOP 1 [Id], [Name], [IsoAlpha2Code] 
        FROM [Lookup].[Country] 
        WHERE IsoAlpha2Code = 'GB' ) AS SI_C ON 1 = 1
GO