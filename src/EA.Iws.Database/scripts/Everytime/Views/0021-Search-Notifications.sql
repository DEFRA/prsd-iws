IF OBJECT_ID('[Search].[Notifications]') IS NULL
    EXEC('CREATE VIEW [Search].[Notifications] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Search].[Notifications]
AS
    SELECT
        N.Id,
        N.NotificationNumber,
        N.CompetentAuthority,
        'Export' AS [ImportOrExport],
        WCI.CodeType,
        WC.Code,
        P.Name AS ProducerName,
        I.Name AS ImporterName,
        SOI_C.Name AS CountryOfImport,
        C.LocalAreaId,
        CON.[From] AS ConsentValidFrom,
        CON.[To] AS ConsentValidTo
    FROM
        [Notification].[Notification] N
        INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
            AND NA.[Status] <> 1
        LEFT JOIN [Notification].[WasteCodeInfo] WCI 
            INNER JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
        ON N.Id = WCI.NotificationId
        LEFT JOIN [Notification].[ProducerCollection] PC
            INNER JOIN [Notification].[Producer] P ON PC.Id = P.ProducerCollectionId
        ON N.Id = PC.NotificationId
        LEFT JOIN [Notification].[Importer] I ON N.Id = I.NotificationId
        LEFT JOIN [Notification].[TransportRoute] TR
            LEFT JOIN [Notification].[StateOfImport] SOI ON TR.Id = SOI.TransportRouteId
            INNER JOIN [Lookup].[Country] SOI_C ON SOI.CountryId = SOI_C.Id
        ON N.Id = TR.NotificationId
        LEFT JOIN [Notification].[Consultation] C ON N.Id = C.NotificationId
        LEFT JOIN [Notification].[Consent] CON ON N.Id = CON.NotificationApplicationId

    UNION

    SELECT
        N.Id,
        N.NotificationNumber,
        N.CompetentAuthority,
        'Import' AS [ImportOrExport],
        WC.CodeType,
        WC.Code,
        P.Name AS ProducerName,
        I.Name AS ImporterName,
        SOI_C.Name AS CountryOfImport,
        C.LocalAreaId,
        CON.[From] AS ConsentValidFrom,
        CON.[To] AS ConsentValidTo
    FROM
        [ImportNotification].[Notification] AS N
        LEFT JOIN [ImportNotification].[WasteType] WT
            INNER JOIN [ImportNotification].[WasteCode] WCI ON WCI.WasteTypeId = WT.Id 
            INNER JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
        ON N.Id = WT.ImportNotificationId
        LEFT JOIN [ImportNotification].[Producer] P ON N.Id = P.ImportNotificationId
        LEFT JOIN [ImportNotification].[Importer] I ON N.Id = I.ImportNotificationId
        LEFT JOIN [ImportNotification].[TransportRoute] TR
            LEFT JOIN [ImportNotification].[StateOfImport] SOI ON TR.Id = SOI.TransportRouteId
            INNER JOIN [Notification].[EntryOrExitPoint] EEP ON SOI.EntryPointId = EEP.Id
            INNER JOIN [Lookup].[Country] SOI_C ON EEP.CountryId = SOI_C.Id
        ON N.Id = TR.ImportNotificationId
        LEFT JOIN [ImportNotification].[Consultation] C ON N.Id = C.NotificationId
        LEFT JOIN [ImportNotification].[Consent] CON ON N.Id = CON.NotificationId
GO