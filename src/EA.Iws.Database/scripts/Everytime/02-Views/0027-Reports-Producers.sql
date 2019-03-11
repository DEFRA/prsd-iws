IF OBJECT_ID('[Reports].[Producers]') IS NULL
    EXEC('CREATE VIEW [Reports].[Producers] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[Producers]
AS
	SELECT
		REPLACE(N.[NotificationNumber], ' ', '') AS [NotificationNumber],
		N.[CompetentAuthority] AS [CompetentAuthorityId],
		E.[Name] AS [NotifierName],
		P.[Name] AS [ProducerName],
		P.[Address1] AS [ProducerAddress1],
		P.[Address2] AS [ProducerAddress2],
		P.[TownOrCity] AS [ProducerTownOrCity],
		P.[PostalCode] AS [ProducerPostCode],
		CASE
			WHEN SiteOfExport.[Id] IS NOT NULL THEN CONCAT(SiteOfExport.[Name], '/', SiteOfExport.[PostalCode])
			ELSE ''
		END AS [SiteOfExport],
		LA.[Name] AS [LocalArea],
		CCT.[Description] AS [WasteType],
		NS.[Description] AS [NotificationStatus],
		I.[Name] AS [ConsigneeName],
		C.[From] AS [ConsentFrom],
        C.[To] AS [ConsentTo],
		D.[NotificationReceivedDate] AS [NotificationReceivedDate],
		MR.[Date] AS [MovementReceivedDate],
        MOR.[Date] AS [MovementCompletedDate],
		WCEWC.Code AS [EwcCode],
		WCYCODE.Code AS [YCode],
		SE_EEP.[Name] AS [PointOfExit],
        SI_EEP.[Name] AS [PointOfEntry],
		SE_C.[Name] AS [ExportCountryName],
        SI_C.[Name] AS [ImportCountryName],
		CASE
			WHEN SiteOfExport.[Id] IS NOT NULL THEN SiteOfExport.[Name]
			ELSE ''
		END AS [SiteOfExportName],
		F.[Name] AS [FacilityName]
	FROM
		[Notification].[Notification] N
		INNER JOIN [Notification].[Exporter] E ON E.NotificationId = N.Id
		INNER JOIN [Notification].[Importer] I ON I.NotificationId = N.Id
		INNER JOIN [Notification].[ProducerCollection] PC ON PC.NotificationId = N.Id
		INNER JOIN [Notification].[Producer] P ON P.ProducerCollectionId = PC.Id
		LEFT JOIN [Notification].[Producer] SiteOfExport
        ON SiteOfExport.Id = 
        (
            SELECT TOP 1 P1.Id

            FROM		[Notification].[ProducerCollection] AS PC

            INNER JOIN [Notification].[Producer] AS P1
            ON		   PC.Id = P1.ProducerCollectionId

            WHERE		PC.NotificationId = N.Id 
						AND [IsSiteOfExport] = 1
            ORDER BY	P1.[IsSiteOfExport] DESC
        )
		LEFT JOIN [Notification].[Consultation] CON 
			INNER JOIN [Lookup].[LocalArea] LA ON CON.LocalAreaId = LA.Id 
		ON CON.NotificationId = N.Id
		INNER JOIN [Notification].[WasteType] WT ON WT.NotificationId = N.Id
		INNER JOIN [Lookup].[ChemicalCompositionType] CCT ON CCT.Id = WT.ChemicalCompositionType
		INNER JOIN [Notification].[NotificationAssessment] NA ON NA.NotificationApplicationId = N.Id
		INNER JOIN	[Lookup].[NotificationStatus] AS NS ON [NS].[Id] = [NA].[Status]
		LEFT JOIN [Notification].[Consent] C ON C.NotificationApplicationId = N.Id
		LEFT JOIN [Notification].NotificationDates D ON D.[NotificationAssessmentId] = NA.[Id]
		LEFT JOIN [Notification].[Movement] M ON M.[NotificationId] = N.Id
		LEFT JOIN [Notification].[MovementReceipt] MR ON MR.MovementId = M.Id
		LEFT JOIN [Notification].[MovementOperationReceipt] MOR ON MOR.MovementId = M.Id
		INNER JOIN [Notification].[TransportRoute] TR ON TR.NotificationId = N.Id
		INNER JOIN [Notification].[StateOfExport] SE ON SE.TransportRouteId = TR.Id
		INNER JOIN [Notification].[StateOfImport] SI ON SI.TransportRouteId = TR.Id
		INNER JOIN [Notification].[EntryOrExitPoint] SE_EEP ON SE_EEP.Id = SE.ExitPointId
		INNER JOIN [Notification].[EntryOrExitPoint] SI_EEP ON SI_EEP.Id = SI.EntryPointId
		INNER JOIN [Lookup].[Country] SI_C ON SI_C.Id = SI.CountryId
		INNER JOIN [Lookup].[Country] SE_C ON SE_C.Id = SE.CountryId
		INNER JOIN [Notification].[FacilityCollection] AS FC ON FC.NotificationId = N.Id
		INNER JOIN [Notification].[Facility] F ON F.FacilityCollectionId = FC.NotificationId
		INNER JOIN [Notification].[WasteCodeInfo] WCI ON WCI.NotificationId = N.Id
		LEFT JOIN [Lookup].[WasteCode] WCEWC ON WCI.WasteCodeId = WCEWC.Id AND WCEWC.CodeType = 3
		LEFT JOIN [Lookup].[WasteCode] WCYCODE ON WCI.WasteCodeId = WCYCODE.Id AND WCYCODE.CodeType = 3

	UNION ALL

	SELECT
		REPLACE(N.[NotificationNumber], ' ', '') AS [NotificationNumber],
		N.[CompetentAuthority] AS [CompetentAuthorityId],
		E.[Name] AS [NotifierName],
		P.[Name] AS [ProducerName],
		P.[Address1] AS [ProducerAddress1],
		P.[Address2] AS [ProducerAddress2],
		P.[TownOrCity] AS [ProducerTownOrCity],
		P.[PostalCode] AS [ProducerPostCode],
		CONCAT(P.[Name], '/', P.[PostalCode]) [SiteOfExport],
		LA.[Name] AS [LocalArea],
		CCT.[Description] AS [WasteType],
		NS.[Description] AS [NotificationStatus],
		I.[Name] AS [ConsigneeName],
		C.[From] AS [ConsentFrom],
        C.[To] AS [ConsentTo],
		D.[NotificationReceivedDate] AS [NotificationReceivedDate],
		MR.[Date] AS [MovementReceivedDate],
        MOR.[Date] AS [MovementCompletedDate],
		WCEWC.Code AS [EwcCode],
		WCYCODE.Code AS [YCode],
		SE_EEP.[Name] AS [PointOfExit],
        SI_EEP.[Name] AS [PointOfEntry],
		SE_C.[Name] AS [ExportCountryName],
        SI_C.[Name] AS [ImportCountryName],
		P.[Name] [SiteOfExportName],
		F.[Name] AS [FacilityName]
	FROM
		[ImportNotification].[Notification] N
		INNER JOIN [ImportNotification].[Exporter] E ON E.ImportNotificationId = N.Id
		INNER JOIN [ImportNotification].[Importer] I ON I.ImportNotificationId = N.Id
		INNER JOIN [ImportNotification].[Producer] P ON P.ImportNotificationId = N.Id
		LEFT JOIN [ImportNotification].[Consultation] CON 
			INNER JOIN [Lookup].[LocalArea] LA ON CON.LocalAreaId = LA.Id
		ON CON.NotificationId = N.Id
		INNER JOIN [ImportNotification].[WasteType] WT ON WT.ImportNotificationId = N.Id
		INNER JOIN [Lookup].[ChemicalCompositionType] CCT ON CCT.Id = WT.ChemicalCompositionType
		INNER JOIN [ImportNotification].[NotificationAssessment] AS NA ON NA.[NotificationApplicationId] = N.Id
		INNER JOIN	[Lookup].[ImportNotificationStatus] AS NS ON [NS].[Id] = [NA].[Status]
		LEFT JOIN [ImportNotification].[Consent] C ON C.NotificationId = N.Id
		LEFT JOIN [ImportNotification].NotificationDates D ON D.[NotificationAssessmentId] = NA.[Id]
		LEFT JOIN [ImportNotification].[Movement] M ON M.[NotificationId] = N.Id
		LEFT JOIN [ImportNotification].[MovementReceipt] MR ON MR.MovementId = M.Id
		LEFT JOIN [ImportNotification].[MovementOperationReceipt] MOR ON MOR.MovementId = M.Id
		INNER JOIN [ImportNotification].[TransportRoute] TR ON TR.ImportNotificationId = N.Id
		INNER JOIN [ImportNotification].[StateOfExport] SE ON SE.TransportRouteId = TR.Id
		INNER JOIN [ImportNotification].[StateOfImport] SI ON SI.TransportRouteId = TR.Id
		INNER JOIN [Notification].[EntryOrExitPoint] SE_EEP ON SE_EEP.Id = SE.ExitPointId
		INNER JOIN [Notification].[EntryOrExitPoint] SI_EEP ON SI_EEP.Id = SI.EntryPointId
		INNER JOIN (
			SELECT TOP 1 [Id], [Name], [IsoAlpha2Code] 
			FROM [Lookup].[Country] 
			WHERE IsoAlpha2Code = 'GB' ) AS SI_C ON 1 = 1
		INNER JOIN [Lookup].[Country] SE_C ON SE_C.Id = SE.CountryId
		INNER JOIN [ImportNotification].[FacilityCollection] AS FC ON FC.ImportNotificationId = N.Id
		INNER JOIN [ImportNotification].[Facility] F ON F.FacilityCollectionId = FC.Id
		INNER JOIN [ImportNotification].[WasteCode] WCI ON WT.Id = WCI.WasteTypeId
		INNER JOIN [Lookup].[WasteCode] WCEWC ON WCI.WasteCodeId = WCEWC.Id AND WCEWC.CodeType = 3
		LEFT JOIN [Lookup].[WasteCode] WCYCODE ON WCI.WasteCodeId = WCYCODE.Id AND WCEWC.CodeType = 4

GO