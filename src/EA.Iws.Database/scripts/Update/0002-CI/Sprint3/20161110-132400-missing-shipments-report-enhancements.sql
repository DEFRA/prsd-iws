IF OBJECT_ID('[Reports].[NotificationShipmentDataMissingShipments]') IS NULL
    EXEC('CREATE VIEW [Reports].[NotificationShipmentDataMissingShipments] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[NotificationShipmentDataMissingShipments]
AS
	
	SELECT	
		M.NotificationId,
		N.NotificationNumber,
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
			WHEN WT.ChemicalCompositionDescription IS NULL THEN CCT.Description
			ELSE CCT.Description + ' - ' + WT.ChemicalCompositionDescription
		END AS [ChemicalComposition],
		LA.[Name] AS [LocalArea],
		SI.Quantity AS TotalQuantity,
		SI_U.Description AS TotalQuantityUnits,
		SI.Units AS TotalQuantityUnitsId,
		IP.Name as EntryPort,
		DC.Name as DestinationCountry,
		OP.Name as ExitPort,
		OC.Name as OriginatingCountry
	
	FROM [Notification].[Movement] AS M

	INNER JOIN [Notification].[Notification] AS N
	ON M.NotificationId = N.Id

	INNER JOIN	[Notification].[WasteType] AS WT 
    ON			[WT].[NotificationId] = M.NotificationId

	INNER JOIN	[Lookup].[ChemicalCompositionType] AS CCT 
    ON			[WT].[ChemicalCompositionType] = [CCT].[Id]

	LEFT JOIN [Notification].[Exporter] AS E
    ON E.[NotificationId] = M.NotificationId

	LEFT JOIN [Notification].[Importer] AS I
    ON I.[NotificationId] = M.NotificationId

	LEFT JOIN	[Notification].[Facility] AS F
    ON			F.Id = 
                (
                    SELECT TOP 1 F1.Id

                    FROM		[Notification].[FacilityCollection] AS FC

                    INNER JOIN	[Notification].[Facility] AS F1
                    ON			FC.Id = F1.FacilityCollectionId

                    WHERE		NotificationId = M.NotificationId
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

	LEFT JOIN	[Notification].[ShipmentInfo] AS SI
	ON			SI.[NotificationId] = M.NotificationId

	LEFT JOIN	[Lookup].[ShipmentQuantityUnit] AS SI_U 
	ON			[SI].[Units] = [SI_U].[Id]

	LEFT JOIN	[Notification].[TransportRoute] AS TR 
	ON			[TR].[NotificationId] = [N].[Id]

	LEFT JOIN   [Notification].[StateOfImport] as SOI
	ON			SOI.TransportRouteId = TR.Id

	LEFT JOIN   [Notification].[EntryOrExitPoint] as IP
	ON			IP.Id = SOI.EntryPointId

	LEFT JOIN   [Lookup].[Country] as DC
	ON          DC.Id = SOI.CountryId

	LEFT JOIN   [Notification].[StateOfExport] as SOO
	ON			SOO.TransportRouteId = TR.Id

	LEFT JOIN   [Notification].[EntryOrExitPoint] as OP
	ON			OP.Id = SOO.ExitPointId

	LEFT JOIN   [Lookup].[Country] as OC
	ON          OC.Id = SOO.CountryId

	UNION 

		SELECT	
		M.NotificationId,
		N.NotificationNumber,
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
		NULL AS [ChemicalComposition],
		LA.[Name] AS [LocalArea],
		SI.Quantity AS TotalQuantity,
		SI_U.Description AS TotalQuantityUnits,
		SI.Units AS TotalQuantityUnitsId,
		IP.Name as EntryPort,
		'United Kingdom' as DestinationCountry,
		OP.Name as ExitPort,
		OC.Name as OriginatingCountry
	
	FROM [ImportNotification].[Movement] AS M

	INNER JOIN [ImportNotification].[Notification] AS N
	ON M.NotificationId = N.Id

	LEFT JOIN [ImportNotification].[Exporter] AS E
    ON E.[ImportNotificationId] = M.NotificationId

	LEFT JOIN [ImportNotification].[Importer] AS I
    ON I.[ImportNotificationId] = M.NotificationId

	LEFT JOIN	[ImportNotification].[Facility] AS F
    ON			F.Id = 
                (
                    SELECT TOP 1 F1.Id

                    FROM		[ImportNotification].[FacilityCollection] AS FC

                    INNER JOIN	[ImportNotification].[Facility] AS F1
                    ON			FC.Id = F1.FacilityCollectionId

                    WHERE		NotificationId = M.NotificationId
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

	LEFT JOIN	[ImportNotification].[Shipment] AS SI
	ON			SI.[ImportNotificationId] = M.NotificationId

	LEFT JOIN	[Lookup].[ShipmentQuantityUnit] AS SI_U 
	ON			[SI].[Units] = [SI_U].[Id]

	LEFT JOIN	[ImportNotification].[TransportRoute] AS TR 
	ON			[TR].[ImportNotificationId] = [N].[Id]

	LEFT JOIN   [ImportNotification].[StateOfImport] as SOI
	on			SOI.TransportRouteId = TR.Id

	LEFT JOIN   [Notification].[EntryOrExitPoint] as IP
	on			IP.Id = SOI.EntryPointId

	LEFT JOIN   [ImportNotification].[StateOfExport] as SOO
	on			SOO.TransportRouteId = TR.Id

	LEFT JOIN   [Notification].[EntryOrExitPoint] as OP
	on			OP.Id = SOO.ExitPointId

	LEFT JOIN   [Lookup].[Country] as OC
	on          OC.Id = SOO.CountryId

GO