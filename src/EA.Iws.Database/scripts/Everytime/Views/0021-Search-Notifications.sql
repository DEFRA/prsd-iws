IF OBJECT_ID('[Search].[Notifications]') IS NULL
    EXEC('CREATE VIEW [Search].[Notifications] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Search].[Notifications]
AS
    SELECT
        N.Id,
        N.NotificationNumber,
        N.CompetentAuthority,
        N.NotificationType,
        NA.Status AS ExportStatus,
        NULL AS ImportStatus,
        ND.NotificationReceivedDate,
        'Export' AS [ImportOrExport],
        WC_Basel.Code AS BaselOecdCode,
        CASE 
            WHEN WCI_Basel.IsNotApplicable = 1 THEN 1
            ELSE 0 END
        AS BaselOecdCodeNotListed,
        WC_Ewc.Code AS EwcCode,
        P.Name AS ProducerName,
        I.Name AS ImporterName,
        E.Name AS ExporterName,
        F.Name AS FacilityName,
        FC.IsInterim,
        SOI_C.Name AS CountryOfImport,
        SOE_C.Name AS CountryOfExport,
        EnPt.Name AS EntryPointName,
        ExPt.Name AS ExitPointName,
        STUFF(( SELECT ',' + CONVERT(NVARCHAR(4), OC.OperationCode) AS [text()]
               FROM [Notification].[OperationCodes] OC
               WHERE OC.NotificationId = N.Id
               ORDER BY OC.OperationCode
               FOR XML PATH('')
             ), 1, 1, '' ) AS OperationCodes,
        C.LocalAreaId,
        CON.[From] AS ConsentValidFrom,
        CON.[To] AS ConsentValidTo
    FROM
        [Notification].[Notification] N
        INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
            AND NA.[Status] <> 1
        INNER JOIN [Notification].[WasteCodeInfo] WCI_Basel
            LEFT JOIN [Lookup].[WasteCode] WC_Basel ON WCI_Basel.WasteCodeId = WC_Basel.Id
        ON N.Id = WCI_Basel.NotificationId AND WCI_Basel.CodeType IN (1, 2)
        INNER JOIN [Notification].[WasteCodeInfo] WCI_Ewc
            LEFT JOIN [Lookup].[WasteCode] WC_Ewc ON WCI_Ewc.WasteCodeId = WC_Ewc.Id
        ON N.Id = WCI_Ewc.NotificationId AND WCI_Ewc.CodeType = 3
        INNER JOIN [Notification].[ProducerCollection] PC
            INNER JOIN [Notification].[Producer] P ON PC.Id = P.ProducerCollectionId
        ON N.Id = PC.NotificationId
        INNER JOIN [Notification].[Importer] I ON N.Id = I.NotificationId
        INNER JOIN [Notification].[TransportRoute] TR
            INNER JOIN [Notification].[StateOfImport] SOI ON TR.Id = SOI.TransportRouteId
            INNER JOIN [Lookup].[Country] SOI_C ON SOI.CountryId = SOI_C.Id
            INNER JOIN [Notification].[StateOfExport] SOE ON TR.Id = SOE.TransportRouteId
            INNER JOIN [Lookup].[Country] SOE_C ON SOE.CountryId = SOE_C.Id
            INNER JOIN [Notification].[EntryOrExitPoint] ExPt ON SOE.ExitPointId = ExPt.Id
            INNER JOIN [Notification].[EntryOrExitPoint] EnPt ON SOI.EntryPointId = EnPt.Id
        ON N.Id = TR.NotificationId
        INNER JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
        INNER JOIN [Notification].[FacilityCollection] FC
            INNER JOIN [Notification].[Facility] F ON FC.Id = F.FacilityCollectionId
        ON N.Id = FC.NotificationId
        INNER JOIN [Notification].[NotificationDates] ND ON NA.Id = ND.NotificationAssessmentId
        LEFT JOIN [Notification].[Consultation] C ON N.Id = C.NotificationId
        LEFT JOIN [Notification].[Consent] CON ON N.Id = CON.NotificationApplicationId

    UNION ALL

    SELECT
        N.Id,
        N.NotificationNumber,
        N.CompetentAuthority,
        N.NotificationType,
        NULL AS ExportStatus,
        NA.Status AS ImportStatus,
        ND.NotificationReceivedDate,
        'Import' AS [ImportOrExport],
        WC_Basel.Code AS BaselOecdCode,
        WT_Basel.BaselOecdCodeNotListed,
        WC_Ewc.Code AS EwcCode,
        P.Name AS ProducerName,
        I.Name AS ImporterName,
        E.Name AS ExporterName,
        F.Name AS FacilityName,
        InS.IsInterim,
        SOI_C.Name AS CountryOfImport,
        SOE_C.Name AS CountryOfExport,
        EnPt.Name AS EntryPointName,
        ExPt.Name AS ExitPointName,
        STUFF(( SELECT ',' + CONVERT(NVARCHAR(4), OC.OperationCode) AS [text()]
               FROM [ImportNotification].[WasteOperation] WO
               INNER JOIN [ImportNotification].[OperationCodes] OC ON WO.Id = OC.WasteOperationId
               WHERE N.Id = WO.ImportNotificationId
               ORDER BY OC.OperationCode
               FOR XML PATH('')
             ), 1, 1, '' ) AS OperationCodes,
        C.LocalAreaId,
        CON.[From] AS ConsentValidFrom,
        CON.[To] AS ConsentValidTo
    FROM
        [ImportNotification].[Notification] AS N
        INNER JOIN [ImportNotification].[WasteType] WT_Basel
            LEFT JOIN [ImportNotification].[WasteCode] WCI_Basel 
                INNER JOIN [Lookup].[WasteCode] WC_Basel ON WCI_Basel.WasteCodeId = WC_Basel.Id AND WC_Basel.CodeType IN (1, 2)
            ON WCI_Basel.WasteTypeId = WT_Basel.Id 
        ON N.Id = WT_Basel.ImportNotificationId
        INNER JOIN [ImportNotification].[WasteType] WT_Ewc
            LEFT JOIN [ImportNotification].[WasteCode] WCI_Ewc 
                INNER JOIN [Lookup].[WasteCode] WC_Ewc ON WCI_Ewc.WasteCodeId = WC_Ewc.Id AND WC_Ewc.CodeType = 3
            ON WCI_Ewc.WasteTypeId = WT_Ewc.Id             
        ON N.Id = WT_Ewc.ImportNotificationId
        INNER JOIN [ImportNotification].[Producer] P ON N.Id = P.ImportNotificationId
        INNER JOIN [ImportNotification].[Importer] I ON N.Id = I.ImportNotificationId
        INNER JOIN [ImportNotification].[TransportRoute] TR
            INNER JOIN [ImportNotification].[StateOfImport] SOI ON TR.Id = SOI.TransportRouteId
            INNER JOIN [Notification].[EntryOrExitPoint] EnPt ON SOI.EntryPointId = EnPt.Id
            INNER JOIN [Lookup].[Country] SOI_C ON EnPt.CountryId = SOI_C.Id
            INNER JOIN [ImportNotification].[StateOfExport] SOE ON TR.Id = SOE.TransportRouteId
            INNER JOIN [Lookup].[Country] SOE_C ON SOE.CountryId = SOE_C.Id
            INNER JOIN [Notification].[EntryOrExitPoint] ExPt ON SOE.ExitPointId = ExPt.Id
        ON N.Id = TR.ImportNotificationId
        INNER JOIN [ImportNotification].[Exporter] E ON N.Id = E.ImportNotificationId
        INNER JOIN [ImportNotification].[FacilityCollection] FC
            INNER JOIN [ImportNotification].[Facility] F ON FC.Id = F.FacilityCollectionId
        ON N.Id = FC.ImportNotificationId
        INNER JOIN [ImportNotification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
        INNER JOIN [ImportNotification].[NotificationDates] ND ON NA.Id = ND.NotificationAssessmentId
        INNER JOIN [ImportNotification].[InterimStatus] InS ON N.Id = InS.ImportNotificationId
        LEFT JOIN [ImportNotification].[Consultation] C ON N.Id = C.NotificationId
        LEFT JOIN [ImportNotification].[Consent] CON ON N.Id = CON.NotificationId
GO