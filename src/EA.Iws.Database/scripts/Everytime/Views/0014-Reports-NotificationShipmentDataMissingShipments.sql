IF OBJECT_ID('[Reports].[NotificationShipmentDataMissingShipments]') IS NULL
    EXEC('CREATE VIEW [Reports].[NotificationShipmentDataMissingShipments] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[NotificationShipmentDataMissingShipments]
AS
    
    SELECT	
        M.NotificationId,
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        N.CompetentAuthority AS CompetentAuthorityId,
        E.Name AS Exporter,
        I.Name AS Importer,
        F.Name AS Facility,
        M.Number AS ShipmentNumber,
        M.Date AS ActualDateOfShipment,
        C.[From] AS [ConsentFrom],
        C.[To] AS [ConsentTo],
        M.PrenotificationDate,
        MR.Date AS ReceivedDate,
        MOR.Date AS CompletedDate,
        MR.Quantity AS QuantityReceived,
        MR_U.Description AS [QuantityReceivedUnit],
        MR_U.Id AS [QuantityReceivedUnitId],
        CASE
            WHEN WT.ChemicalCompositionType = 4 THEN CCT.Description + ' - ' + WT.ChemicalCompositionName
            WHEN WT.ChemicalCompositionDescription IS NULL THEN CCT.Description
            ELSE CCT.Description + ' - ' + WT.ChemicalCompositionDescription
        END AS [ChemicalComposition],
        LA.[Name] AS [LocalArea],
        SI.Quantity AS TotalQuantity,
        SI_U.Description AS TotalQuantityUnits,
        SI.Units AS TotalQuantityUnitsId,
        TR.[EntryPoint] AS EntryPort,
        TR.[ImportCountryName] AS DestinationCountry,
        TR.[ExitPoint] AS ExitPort,
        TR.[ExportCountryName] AS OriginatingCountry,
        MS.Status,
        ND.[NotificationReceivedDate],
        STUFF(( SELECT ', ' + WC.Code AS [text()]
                FROM [Notification].[WasteCodeInfo] WCI
                LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
                WHERE WCI.NotificationId = M.NotificationId AND WC.CodeType = 3
                order by 1
                FOR XML PATH('')
                ), 1, 1, '' ) AS [EwcCodes]
    
    FROM [Notification].[Movement] AS M

    INNER JOIN [Notification].[Notification] AS N
    ON M.NotificationId = N.Id

    INNER JOIN	[Notification].[WasteType] AS WT 
    ON			[WT].[NotificationId] = M.NotificationId

    INNER JOIN	[Lookup].[ChemicalCompositionType] AS CCT 
    ON			[WT].[ChemicalCompositionType] = [CCT].[Id]

    INNER JOIN [Notification].[Exporter] AS E
    ON E.[NotificationId] = M.NotificationId

    INNER JOIN [Notification].[Importer] AS I
    ON I.[NotificationId] = M.NotificationId

    INNER JOIN	[Notification].[Facility] AS F
    ON			F.Id = 
                (
                    SELECT TOP 1 F1.Id

                    FROM		[Notification].[FacilityCollection] AS FC

                    INNER JOIN	[Notification].[Facility] AS F1
                    ON			FC.Id = F1.FacilityCollectionId

                    WHERE		FC.NotificationId = M.NotificationId
                    ORDER BY	F1.IsActualSiteOfTreatment DESC
                )

    LEFT JOIN	[Notification].[MovementReceipt] AS MR
    ON			[M].[Id] = [MR].[MovementId]

    LEFT JOIN	[Notification].[MovementOperationReceipt] AS MOR
    ON			[M].[Id] = [MOR].[MovementId]

    LEFT JOIN	[Lookup].[ShipmentQuantityUnit] AS MR_U 
    ON			[MR].[Unit] = [MR_U].[Id]

    LEFT JOIN	[Notification].[Consent] AS C
    ON			M.NotificationId = [C].[NotificationApplicationId]

    LEFT JOIN	[Notification].[Consultation] AS CON
    ON			[CON].[NotificationId] = M.NotificationId

    LEFT JOIN	[Lookup].[LocalArea] AS LA
    ON			[CON].[LocalAreaId] = [LA].[Id]

    INNER JOIN	[Notification].[ShipmentInfo] AS SI
    ON			SI.[NotificationId] = M.NotificationId

    INNER JOIN	[Lookup].[ShipmentQuantityUnit] AS SI_U 
    ON			[SI].[Units] = [SI_U].[Id]

    INNER JOIN   [Reports].[TransportRoute] AS TR
    ON			[TR].[NotificationId] = [N].[Id]

    LEFT JOIN   [Lookup].[MovementStatus] AS MS
    ON			MS.Id = M.Status

    INNER JOIN	[Notification].[NotificationAssessment] AS NA
    ON			NA.NotificationApplicationId = N.Id

    INNER JOIN	[Notification].[NotificationDates] AS ND
    ON			ND.[NotificationAssessmentId] = NA.Id

    UNION ALL

    SELECT	
        M.NotificationId,
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        N.CompetentAuthority AS CompetentAuthorityId,
        E.Name AS Exporter,
        I.Name AS Importer,
        F.Name AS Facility,
        M.Number AS ShipmentNumber,
        M.ActualShipmentDate AS ActualDateOfShipment,
        C.[From] AS [ConsentFrom],
        C.[To] AS [ConsentTo],
        M.PrenotificationDate,
        MR.Date AS ReceivedDate,
        MOR.Date AS CompletedDate,
        MR.Quantity AS QuantityReceived,
        MR_U.Description AS [QuantityReceivedUnit],
        MR_U.Id AS [QuantityReceivedUnitId],
        CASE
            WHEN WT.Name IS NULL THEN CCT.Description
            ELSE CCT.Description + ' - ' + WT.Name
        END AS [ChemicalComposition],
        LA.[Name] AS [LocalArea],
        SI.Quantity AS TotalQuantity,
        SI_U.Description AS TotalQuantityUnits,
        SI.Units AS TotalQuantityUnitsId,
        TR.EntryPoint AS EntryPort,
        TR.ImportCountryName AS DestinationCountry,
        TR.ExitPoint AS ExitPort,
        TR.ExportCountryName AS OriginatingCountry,
        'NA' AS Status,
        ND.[NotificationReceivedDate],
        STUFF(( SELECT ', ' + WC.Code AS [text()]
                FROM [ImportNotification].[WasteType] WT
                INNER JOIN [ImportNotification].[WasteCode] WCI ON WT.Id = WCI.WasteTypeId
                LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
                WHERE WT.ImportNotificationId = M.NotificationId AND WC.CodeType = 3
                order by 1
                FOR XML PATH('')
                ), 1, 1, '' ) AS [EwcCodes]
    
    FROM [ImportNotification].[Movement] AS M

    INNER JOIN [ImportNotification].[Notification] AS N
    ON M.NotificationId = N.Id

    INNER JOIN	[ImportNotification].[WasteType] AS WT 
    ON			[WT].[ImportNotificationId] = M.NotificationId

    INNER JOIN	[Lookup].[ChemicalCompositionType] AS CCT 
    ON			[WT].[ChemicalCompositionType] = [CCT].[Id]

    INNER JOIN [ImportNotification].[Exporter] AS E
    ON E.[ImportNotificationId] = M.NotificationId

    INNER JOIN [ImportNotification].[Importer] AS I
    ON I.[ImportNotificationId] = M.NotificationId

    INNER JOIN	[ImportNotification].[Facility] AS F
    ON			F.Id = 
                (
                    SELECT TOP 1 F1.Id

                    FROM		[ImportNotification].[FacilityCollection] AS FC

                    INNER JOIN	[ImportNotification].[Facility] AS F1
                    ON			FC.Id = F1.FacilityCollectionId

                    WHERE		FC.ImportNotificationId = M.NotificationId
                    ORDER BY	F1.IsActualSiteOfTreatment DESC
                )

    LEFT JOIN	[ImportNotification].[MovementReceipt] AS MR
    ON			[M].[Id] = [MR].[MovementId]

    LEFT JOIN	[ImportNotification].[MovementOperationReceipt] AS MOR
    ON			[M].[Id] = [MOR].[MovementId]

    LEFT JOIN	[Lookup].[ShipmentQuantityUnit] AS MR_U 
    ON			[MR].[Unit] = [MR_U].[Id]

    LEFT JOIN	[ImportNotification].[Consent] AS C
    ON			M.NotificationId = [C].[NotificationId]

    LEFT JOIN	[ImportNotification].[Consultation] AS CON
    ON			[CON].[NotificationId] = M.NotificationId

    LEFT JOIN	[Lookup].[LocalArea] AS LA
    ON			[CON].[LocalAreaId] = [LA].[Id]

    INNER JOIN	[ImportNotification].[Shipment] AS SI
    ON			SI.[ImportNotificationId] = M.NotificationId

    INNER JOIN	[Lookup].[ShipmentQuantityUnit] AS SI_U 
    ON			[SI].[Units] = [SI_U].[Id]

    LEFT JOIN   [Reports].[TransportRoute] AS TR
    ON			[TR].[NotificationId] = [N].[Id]

    INNER JOIN	[ImportNotification].[NotificationAssessment] AS NA
    ON			NA.NotificationApplicationId = N.Id

    INNER JOIN	[ImportNotification].[NotificationDates] AS ND
    ON			ND.[NotificationAssessmentId] = NA.Id

GO