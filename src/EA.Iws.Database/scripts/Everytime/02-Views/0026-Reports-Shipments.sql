IF OBJECT_ID('[Reports].[Shipments]') IS NULL
    EXEC('CREATE VIEW [Reports].[Shipments] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[Shipments]
AS
	SELECT	
        M.NotificationId,
        'Export' AS [ImportOrExport],
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        N.CompetentAuthority AS CompetentAuthorityId,
        E.Name AS Exporter,
        E.Type AS NotifierCompanyType,
        I.Name AS Importer,
		I.Type AS ConsigneeCompanyType,
        F.Name AS Facility,
		F.Type AS FacilityCompanyType,
        COALESCE(BaselCodeInfo.[Code] + ' - ' + BaselCodeInfo.[Description], 'Not listed') AS [BaselOecdCode],
        M.Number AS ShipmentNumber,
        M.Date AS ActualDateOfShipment,
        C.[From] AS [ConsentFrom],
        C.[To] AS [ConsentTo],
        M.PrenotificationDate,
        CASE
			WHEN MR.Date IS NULL THEN MPR.WasteReceivedDate ELSE MR.Date 
		END AS [ReceivedDate],
        MOR.Date AS CompletedDate,
		CASE 
			WHEN MS.Status = 'Rejected' THEN
				MREJECT.RejectedQuantity				
			ELSE
				MPR.RejectedQuantity END AS [RejectedQuantity],
		CASE 
			WHEN MS.Status = 'Rejected' THEN
				MREJECT.Date 
			ELSE
				MPR.WasteReceivedDate END AS [ShipmentRejectedDate],
        CASE 
			WHEN MS.Status = 'Rejected' THEN
				MREJECT.Reason
			ELSE
				MPR.Reason END AS [RejectedReason],
        CASE
			WHEN MR.Quantity IS NULL THEN MPR.ActualQuantity ELSE MR.Quantity 
		END AS [QuantityReceived],
        CASE
			WHEN MR_U.Description IS NULL THEN MPR_U.Description ELSE MR_U.Description  
		END AS [QuantityReceivedUnit],
        CASE
			WHEN MR_U.Id IS NULL THEN MPR_U.Id ELSE MR_U.Id  
		END AS [QuantityReceivedUnitId],
        WT.[ChemicalCompositionType] AS [ChemicalCompositionTypeId],
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
                ), 1, 1, '' ) AS [EwcCodes],
        STUFF(( SELECT ', ' + O.Name AS [text()]
            FROM [Notification].[OperationCodes] OC
            INNER JOIN [Lookup].[OperationCode] O ON OC.OperationCode = O.Id
            WHERE OC.NotificationId = N.Id
            ORDER BY O.Id
            FOR XML PATH('')
            ), 1, 1, '' ) AS OperationCodes,
        STUFF(( SELECT ', ' + WC.Code AS [text()]
            FROM [Notification].[WasteCodeInfo] WCI
            LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
            WHERE WCI.NotificationId = N.Id AND WC.CodeType = 4
            order by 1
            FOR XML PATH('')
            ), 1, 1, '' ) AS [YCode],
        STUFF(( SELECT ', ' + WC.Code AS [text()]
            FROM [Notification].[WasteCodeInfo] WCI
            LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
            WHERE WCI.NotificationId = N.Id AND WC.CodeType = 5
            order by 1
            FOR XML PATH('')
            ), 1, 1, '' ) AS [HCode],
        STUFF(( SELECT ', ' + WC.Code AS [text()]
            FROM [Notification].[WasteCodeInfo] WCI
            LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
            WHERE WCI.NotificationId = N.Id AND WC.CodeType = 6
            order by 1
            FOR XML PATH('')
            ), 1, 1, '' ) AS [UNClass],
		CASE
			WHEN SiteOfExport.[Id] IS NOT NULL THEN SiteOfExport.[Name]
			ELSE ''
		END AS [SiteOfExportName],
        'N' AS [ActionedByExternalUser]
    
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

    LEFT JOIN	[Notification].[MovementPartialRejection] AS MPR
    ON			[M].[Id] = [MPR].[MovementId]

	LEFT JOIN	[Notification].[MovementRejection] AS MREJECT
	ON			[M].[Id] = [MREJECT].[MovementId]

    LEFT JOIN	[Lookup].[ShipmentQuantityUnit] AS MR_U 
    ON			[MR].[Unit] = [MR_U].[Id]

    LEFT JOIN	[Lookup].[ShipmentQuantityUnit] AS MPR_U 
    ON			[MPR].[ActualUnit] = [MPR_U].[Id]

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

    LEFT JOIN   [Notification].[WasteCodeInfo] BaselCode
                LEFT JOIN [Lookup].[WasteCode] BaselCodeInfo ON BaselCode.WasteCodeId = BaselCodeInfo.Id
    ON          BaselCode.NotificationId = N.Id AND BaselCode.CodeType IN (1, 2)

	LEFT JOIN	[Notification].[Producer] AS SiteOfExport
	ON			SiteOfExport.Id = 
				(
					SELECT TOP 1 P1.Id

					FROM		[Notification].[ProducerCollection] AS PC

					INNER JOIN [Notification].[Producer] AS P1
					ON		   PC.Id = P1.ProducerCollectionId

					WHERE		PC.NotificationId = N.Id 
								AND [IsSiteOfExport] = 1
					ORDER BY	P1.[IsSiteOfExport] DESC
				)    

    UNION ALL

    SELECT	
        M.NotificationId,
        'Import' AS [ImportOrExport],
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        N.CompetentAuthority AS CompetentAuthorityId,
        E.Name AS Exporter,
        E.Type AS NotifierCompanyType,
        I.Name AS Importer,
		I.Type AS ConsigneeCompanyType,
        F.Name AS Facility,
		F.Type AS FacilityCompanyType,
        COALESCE(WasteCodeInfo.[Code] + ' - ' + WasteCodeInfo.[Description], 'Not listed') AS [BaselOecdCode],
        M.Number AS ShipmentNumber,
        M.ActualShipmentDate AS ActualDateOfShipment,
        C.[From] AS [ConsentFrom],
        C.[To] AS [ConsentTo],
        M.PrenotificationDate,
        CASE
			WHEN 
				MR.Date IS NULL THEN MPR.WasteReceivedDate 
			ELSE 
				MR.Date 
		END AS [ReceivedDate],
        MOR.Date AS CompletedDate,
		CASE WHEN MREJECT.RejectedQuantity IS NOT NULL THEN MREJECT.RejectedQuantity ELSE MPR.RejectedQuantity END AS [RejectedQuantity],
		CASE WHEN MREJECT.Date IS NOT NULL THEN MREJECT.Date ELSE MPR.WasteReceivedDate END AS [ShipmentRejectedDate],		
        CASE 
			WHEN 
				MREJECT.Reason IS NOT NULL THEN MREJECT.Reason
			ELSE
				MPR.Reason
		END AS [RejectedReason],
		CASE
			WHEN 
				MR.Quantity IS NULL THEN MPR.ActualQuantity
			ELSE 
				MR.Quantity 
		END AS [QuantityReceived],
		CASE
			WHEN 
				MR_U.Description IS NULL THEN MPR_U.Description
			ELSE 
				MR_U.Description  
		END AS [QuantityReceivedUnit],
        CASE
			WHEN 
				MR_U.Id IS NULL THEN MPR_U.Id 
			ELSE MR_U.Id  
		END AS [QuantityReceivedUnitId],
        WT.[ChemicalCompositionType] AS [ChemicalCompositionTypeId],
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
        CASE 
			WHEN M.[IsCancelled] = 1 THEN 'Cancelled'
			ELSE 
				CASE WHEN MR.Quantity IS NOT NULL THEN 'Received'
					ELSE 
						CASE WHEN MREJECT.RejectedQuantity IS NOT NULL THEN 'Rejected'
					ELSE 
						CASE WHEN MPR.WasteDisposedDate IS NOT NULL THEN 'Completed'
						ELSE 'PartiallyRejected'
						END
				END
			END
		END AS [Status],
        ND.[NotificationReceivedDate],
        STUFF(( SELECT ', ' + WC.Code AS [text()]
                FROM [ImportNotification].[WasteType] WT
                INNER JOIN [ImportNotification].[WasteCode] WCI ON WT.Id = WCI.WasteTypeId
                LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
                WHERE WT.ImportNotificationId = M.NotificationId AND WC.CodeType = 3
                order by 1
                FOR XML PATH('')
                ), 1, 1, '' ) AS [EwcCodes],
        STUFF(( SELECT ', ' + O.Name AS [text()]
            FROM [ImportNotification].[WasteOperation] WO
            INNER JOIN [ImportNotification].[OperationCodes] OC ON OC.WasteOperationId = WO.Id
            LEFT JOIN [Lookup].[OperationCode] O ON OC.OperationCode = O.Id
            WHERE WO.ImportNotificationId = N.Id
            ORDER BY O.Id
            FOR XML PATH('')
            ), 1, 1, '' ) AS OperationCodes,
        STUFF(( SELECT ', ' + WC.Code AS [text()]
            FROM [ImportNotification].[WasteType] WT
            INNER JOIN [ImportNotification].[WasteCode] WCI ON WT.Id = WCI.WasteTypeId
            LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
            WHERE WT.ImportNotificationId = N.Id AND WC.CodeType = 4
            order by 1
            FOR XML PATH('')
            ), 1, 1, '' ) AS [YCode],
        STUFF(( SELECT ', ' + WC.Code AS [text()]
            FROM [ImportNotification].[WasteType] WT
            INNER JOIN [ImportNotification].[WasteCode] WCI ON WT.Id = WCI.WasteTypeId
            LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
            WHERE WT.ImportNotificationId = N.Id AND WC.CodeType = 5
            order by 1
            FOR XML PATH('')
            ), 1, 1, '' ) AS [HCode],
        STUFF(( SELECT ', ' + WC.Code AS [text()]
            FROM [ImportNotification].[WasteType] WT
            INNER JOIN [ImportNotification].[WasteCode] WCI ON WT.Id = WCI.WasteTypeId
            INNER JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
            WHERE WT.ImportNotificationId = N.Id AND WC.CodeType = 6
            order by 1
            FOR XML PATH('')
            ), 1, 1, '' ) AS [UNClass],
		P.[Name] [SiteOfExportName],
        'N/A' AS [ActionedByExternalUser]
    
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

	INNER JOIN	[ImportNotification].[Producer] AS P 
	ON			P.ImportNotificationId = N.Id

    LEFT JOIN	[ImportNotification].[MovementReceipt] AS MR
    ON			[M].[Id] = [MR].[MovementId]

    LEFT JOIN	[ImportNotification].[MovementOperationReceipt] AS MOR
    ON			[M].[Id] = [MOR].[MovementId]

	LEFT JOIN	[ImportNotification].[MovementPartialRejection] AS MPR
    ON			[M].[Id] = [MPR].[MovementId]

	LEFT JOIN	[ImportNotification].[MovementRejection] AS MREJECT
	ON			[M].[Id] = [MREJECT].[MovementId]

    LEFT JOIN	[Lookup].[ShipmentQuantityUnit] AS MR_U 
    ON			[MR].[Unit] = [MR_U].[Id]

	LEFT JOIN	[Lookup].[ShipmentQuantityUnit] AS MPR_U
    ON			[MPR].[ActualUnit] = [MPR_U].[Id]

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

    LEFT JOIN   [ImportNotification].[WasteType] WasteType
                INNER JOIN [ImportNotification].[WasteCode] WasteCode ON WasteType.Id = WasteCode.WasteTypeId
                LEFT JOIN [Lookup].[WasteCode] WasteCodeInfo ON WasteCode.WasteCodeId = WasteCodeInfo.Id
    ON			WasteType.ImportNotificationId = N.Id AND WasteCodeInfo.CodeType IN (1, 2)
GO