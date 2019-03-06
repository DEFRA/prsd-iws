IF OBJECT_ID('[Reports].[FreedomOfInformation]') IS NULL
    EXEC('CREATE VIEW [Reports].[FreedomOfInformation] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[FreedomOfInformation]
AS
    SELECT
        REPLACE(N.[NotificationNumber], ' ', '') AS [NotificationNumber],
        'Export' AS [ImportOrExport],
        FC.[IsInterim],
        D.[NotificationReceivedDate] AS [ReceivedDate],
        N.[CompetentAuthority] AS [CompetentAuthorityId],
        E.[Name] AS [NotifierName],
        [Reports].[ConcatenateAddress](E.[Address1], E.[Address2], E.[TownOrCity], E.[PostalCode], E.[Region], E.[Country]) AS [NotifierAddress],
        E.[PostalCode] AS [NotifierPostalCode],
        P.[Name] AS [ProducerName],
        [Reports].[ConcatenateAddress](P.[Address1], P.[Address2], P.[TownOrCity], P.[PostalCode], P.[Region], P.[Country]) AS [ProducerAddress],
        P.[PostalCode] AS [ProducerPostalCode],
        SE_EEP.[Name] AS [PointOfExport],
        SI_EEP.[Name] AS [PointOfEntry],
        SE_C.[Name] AS [ExportCountryName],
        SI_C.[Name] AS [ImportCountryName],
        WT.[ChemicalCompositionType] AS [ChemicalCompositionTypeId],
        COALESCE(BaselCodeInfo.[Code] + ' - ' + BaselCodeInfo.[Description], 'Not listed') AS [BaselOecdCode],
        CASE WHEN WT.[ChemicalCompositionType] = 4
            THEN 'Other - ' + WT.[ChemicalCompositionName]
            ELSE CCT.[Description] END AS [NameOfWaste],
        STUFF(( SELECT ', ' + WC.Code AS [text()]
            FROM [Notification].[WasteCodeInfo] WCI
            LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
            WHERE WCI.NotificationId = N.Id AND WC.CodeType = 3
            order by 1
            FOR XML PATH('')
            ), 1, 1, '' ) AS [EWC],
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
        STUFF(( SELECT ', ' + O.Name AS [text()]
            FROM [Notification].[OperationCodes] OC
            INNER JOIN [Lookup].[OperationCode] O ON OC.OperationCode = O.Id
            WHERE OC.NotificationId = N.Id
            ORDER BY O.Id
            FOR XML PATH('')
            ), 1, 1, '' ) AS OperationCodes,
        I.[Name] AS [ImporterName],
        [Reports].[ConcatenateAddress](I.[Address1], I.[Address2], I.[TownOrCity], I.[PostalCode], I.[Region], I.[Country]) AS [ImporterAddress],
        I.[PostalCode] AS [ImporterPostalCode],
        F.[Name] AS [FacilityName],
        [Reports].[ConcatenateAddress](F.[Address1], F.[Address2], F.[TownOrCity], F.[PostalCode], F.[Region], F.[Country]) AS [FacilityAddress],
        F.[PostalCode] AS [FacilityPostalCode],
        S.[Quantity] AS [IntendedQuantity],
        SU.[Description] AS [IntendedQuantityUnit],
        S.[Units] AS [IntendedQuantityUnitId],
        C.[From] AS [ConsentFrom],
        C.[To] AS [ConsentTo],
        LA.[Name] AS [LocalArea],
        M.[Number] AS [MovementNumber],
        MR.[Date] AS [MovementReceivedDate],
        MOR.[Date] AS [MovementCompletedDate],
        MR.[Unit] AS [MovementQuantityReceviedUnitId],
        MR.[Quantity] AS [MovementQuantityReceived],
		ET.[Description] AS [NotifierType],
		E.[FullName] As [NotifierContactName],
		E.[Email] as [NotifierContactEmail],
		PT.[Description] AS [ProducerType],
		P.[Email] AS [ProducerContactEmail],
		ETS.TransitStates AS [TransitStates],
		IT.[Description] AS [ImporterType],
		I.[FullName] AS [ImporterContactName],
		I.[Email] AS [ImporterContactEmail],
		NS.[Description] AS [NotificationStatus],
		D.DecisionRequiredByDate,
		CASE when FG.[Status] = 4 Then 'Y' ELSE 'N' END AS [IsFinancialGuaranteeApproved],
		D.[FileClosedDate],
		TE.[Details] AS [TechnologyEmployed],
		M.[Date] AS [ActualDate],
		D.AcknowledgedDate,
		D.ObjectedDate AS [ObjectionDate],
		D.WithdrawnDate
    FROM [Notification].[Notification] N
    INNER JOIN [Notification].[FacilityCollection] FC ON FC.[NotificationId] = N.[Id]
    INNER JOIN [Notification].[NotificationAssessment] NA ON NA.[NotificationApplicationId] = N.[Id]
    LEFT JOIN [Notification].NotificationDates D ON D.[NotificationAssessmentId] = NA.[Id]
    INNER JOIN [Notification].[Exporter] E ON E.NotificationId = N.Id
    INNER JOIN [Notification].[Importer] I ON I.NotificationId = N.Id
    INNER JOIN [Notification].[Producer] P
        ON P.Id = 
        (
            SELECT TOP 1 P1.Id

            FROM		[Notification].[ProducerCollection] AS PC

            INNER JOIN [Notification].[Producer] AS P1
            ON		   PC.Id = P1.ProducerCollectionId

            WHERE		PC.NotificationId = N.Id
            ORDER BY	P1.[IsSiteOfExport] DESC
        )
    INNER JOIN [Notification].[Facility] F
        ON F.Id = 
        (
            SELECT TOP 1 F1.Id

            FROM		[Notification].[FacilityCollection] AS FC

            INNER JOIN	[Notification].[Facility] AS F1
            ON			FC.Id = F1.FacilityCollectionId

            WHERE		NotificationId = N.Id
            ORDER BY	F1.IsActualSiteOfTreatment DESC
        )
    INNER JOIN [Notification].[TransportRoute] TR ON TR.NotificationId = N.Id
    INNER JOIN [Notification].[StateOfExport] SE ON SE.TransportRouteId = TR.Id
    INNER JOIN [Notification].[StateOfImport] SI ON SI.TransportRouteId = TR.Id
    INNER JOIN [Notification].[EntryOrExitPoint] SE_EEP ON SE_EEP.Id = SE.ExitPointId
    INNER JOIN [Notification].[EntryOrExitPoint] SI_EEP ON SI_EEP.Id = SI.EntryPointId
    INNER JOIN [Lookup].[Country] SI_C ON SI_C.Id = SI.CountryId
    INNER JOIN [Lookup].[Country] SE_C ON SE_C.Id = SE.CountryId
	INNER JOIN [Lookup].[BusinessType] ET ON ET.Id = E.[Type]
	INNER JOIN [Lookup].[BusinessType] PT ON PT.Id = P.[Type] 
	INNER JOIN [Lookup].[BusinessType] IT ON IT.Id = I.[Type] 
    INNER JOIN [Notification].[WasteType] WT ON WT.NotificationId = N.Id
    INNER JOIN [Lookup].[ChemicalCompositionType] CCT ON CCT.Id = WT.ChemicalCompositionType
    INNER JOIN [Notification].[ShipmentInfo] S ON S.NotificationId = N.Id
    INNER JOIN [Lookup].[ShipmentQuantityUnit] SU ON SU.Id = S.Units
    INNER JOIN	[Lookup].[NotificationStatus] NS  ON [NS].[Id] = [NA].[Status]
    INNER JOIN	[Notification].[TechnologyEmployed] TE ON TE.NotificationId = N.Id
    LEFT JOIN [Notification].[WasteCodeInfo] BaselCode
        LEFT JOIN [Lookup].[WasteCode] BaselCodeInfo ON BaselCode.WasteCodeId = BaselCodeInfo.Id
    ON BaselCode.NotificationId = N.Id AND BaselCode.CodeType IN (1, 2)
    LEFT JOIN [Notification].[Consent] C ON C.NotificationApplicationId = N.Id
    LEFT JOIN [Notification].[Consultation] CON 
        INNER JOIN [Lookup].[LocalArea] LA ON CON.LocalAreaId = LA.Id
    ON CON.NotificationId = N.Id
    LEFT JOIN [Notification].[Movement] M ON M.[NotificationId] = N.Id
    LEFT JOIN [Notification].[MovementReceipt] MR ON MR.MovementId = M.Id
    LEFT JOIN [Notification].[MovementOperationReceipt] MOR ON MOR.MovementId = M.Id
	LEFT JOIN [Reports].[TransitStatesConcat] ETS on ETS.NotificationId = N.Id
	LEFT JOIN  [Notification].[FinancialGuaranteeCollection] FGC ON FGC.[NotificationId] = N.Id
	LEFT JOIN [Notification].[FinancialGuarantee] FG ON FG.[FinancialGuaranteeCollectionId] = FGC.[Id]

    UNION ALL

    SELECT
        REPLACE(N.[NotificationNumber], ' ', '') AS [NotificationNumber],
        'Import' AS [ImportOrExport],
        InS.[IsInterim],
        D.[NotificationReceivedDate] AS [ReceivedDate],
        N.[CompetentAuthority] AS [CompetentAuthorityId],
        E.[Name] AS [NotifierName],
        [Reports].[ConcatenateAddress](E.[Address1], E.[Address2], E.[TownOrCity], E.[PostalCode], NULL, C_E.[Name]) AS [NotifierAddress],
        E.[PostalCode] AS [NotifierPostalCode],
        P.[Name] AS [ProducerName],
        [Reports].[ConcatenateAddress](P.[Address1], P.[Address2], P.[TownOrCity], P.[PostalCode], NULL, C_P.[Name]) AS [ProducerAddress],
        P.[PostalCode] AS [ProducerPostalCode],
        SE_EEP.[Name] AS [PointOfExport],
        SI_EEP.[Name] AS [PointOfEntry],
        SE_C.[Name] AS [ExportCountryName],
        SI_C.[Name] AS [ImportCountryName],
        WT.[ChemicalCompositionType] AS [ChemicalCompositionTypeId],
        COALESCE(WasteCodeInfo.[Code] + ' - ' + WasteCodeInfo.[Description], 'Not listed') AS [BaselOecdCode],
        CASE WHEN WT.[ChemicalCompositionType] = 4
            THEN 'Other - ' + WT.[Name]
            ELSE CCT.[Description] END AS [NameOfWaste],
        STUFF(( SELECT ', ' + WC.Code AS [text()]
            FROM [ImportNotification].[WasteType] WT
            INNER JOIN [ImportNotification].[WasteCode] WCI ON WT.Id = WCI.WasteTypeId
            INNER JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
            WHERE WT.ImportNotificationId = N.Id AND WC.CodeType = 3
            order by 1
            FOR XML PATH('')
            ), 1, 1, '' ) AS [EWC],
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
        STUFF(( SELECT ', ' + O.Name AS [text()]
            FROM [ImportNotification].[WasteOperation] WO
            INNER JOIN [ImportNotification].[OperationCodes] OC ON OC.WasteOperationId = WO.Id
            LEFT JOIN [Lookup].[OperationCode] O ON OC.OperationCode = O.Id
            WHERE WO.ImportNotificationId = N.Id
            ORDER BY O.Id
            FOR XML PATH('')
            ), 1, 1, '' ) AS OperationCodes,
        I.[Name] AS [ImporterName],
        [Reports].[ConcatenateAddress](I.[Address1], I.[Address2], I.[TownOrCity], I.[PostalCode], NULL, C_I.[Name]) AS [ImporterAddress],
        I.[PostalCode] AS [ImporterPostalCode],
        F.[Name] AS [FacilityName],
        [Reports].[ConcatenateAddress](F.[Address1], F.[Address2], F.[TownOrCity], F.[PostalCode], NULL, C_F.[Name]) AS [FacilityAddress],
        F.[PostalCode] AS [FacilityPostalCode],
        S.[Quantity] AS [IntendedQuantity],
        SU.[Description] AS [IntendedQuantityUnit],
        S.[Units] AS [IntendedQuantityUnitId],
        C.[From] AS [ConsentFrom],
        C.[To] AS [ConsentTo],
        LA.[Name] AS [LocalArea],
        M.[Number] AS [MovementNumber],
        MR.[Date] AS [MovementReceivedDate],
        MOR.[Date] AS [MovementCompletedDate],
        MR.[Unit] AS [MovementQuantityReceviedUnitId],
        MR.[Quantity] AS [MovementQuantityReceived],
		NULL AS [NotifierType],
		E.[ContactName] As [NotifierContactName],
		E.[Email] as [NotifierContactEmail],
		NULL AS [ProducerType],
		P.[Email] AS [ProducerContactEmail],
		ETS.TransitStates AS [TransitStates],
		IT.[Description] AS [ImporterType],
		I.[ContactName] AS [ImporterContactName],
		I.[Email] AS [ImporterContactEmail],
		NS.[Description] as [NotificationStatus],
		D.DecisionRequiredByDate,
		CASE when FG.[Status] = 4 Then 'Y' ELSE 'N' END AS [IsFinancialGuaranteeApproved],
		D.FileClosedDate,
	    WO.TechnologyEmployed,
		M.ActualShipmentDate AS [ActualDate],
		D.AcknowledgedDate,
		O.[Date] AS [ObjectionDate],
		D.WithdrawnDate 
    FROM [ImportNotification].[Notification] N
    INNER JOIN [ImportNotification].[FacilityCollection] FC ON FC.[ImportNotificationId] = N.[Id]
    INNER JOIN [ImportNotification].[NotificationAssessment] NA ON NA.[NotificationApplicationId] = N.[Id]
    LEFT JOIN [ImportNotification].NotificationDates D ON D.[NotificationAssessmentId] = NA.[Id]
    INNER JOIN [ImportNotification].[Exporter] E
        INNER JOIN [Lookup].[Country] AS C_E ON E.[CountryId] = C_E.[Id]
    ON E.ImportNotificationId = N.Id
    INNER JOIN [ImportNotification].[Importer] I
        INNER JOIN [Lookup].[Country] AS C_I ON I.[CountryId] = C_I.[Id]
    ON I.ImportNotificationId = N.Id
    INNER JOIN [ImportNotification].[Producer] P
        INNER JOIN [Lookup].[Country] AS C_P ON P.[CountryId] = C_P.[Id]
    ON P.ImportNotificationId = N.Id
    INNER JOIN [ImportNotification].[Facility] F
        ON F.Id = 
        (
            SELECT TOP 1 F1.Id

            FROM		[ImportNotification].[FacilityCollection] AS FC

            INNER JOIN	[ImportNotification].[Facility] AS F1
            ON			FC.Id = F1.FacilityCollectionId

            WHERE		ImportNotificationId = N.Id
            ORDER BY	F1.IsActualSiteOfTreatment DESC
        )
    INNER JOIN [Lookup].[Country] AS C_F ON F.[CountryId] = C_F.[Id]
	INNER JOIN [Lookup].[BusinessType] IT ON IT.Id = I.[Type] 
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
    INNER JOIN [ImportNotification].[WasteType] WT ON WT.ImportNotificationId = N.Id
    INNER JOIN [Lookup].[ChemicalCompositionType] CCT ON CCT.Id = WT.ChemicalCompositionType
    INNER JOIN [ImportNotification].[Shipment] S ON S.ImportNotificationId = N.Id
    INNER JOIN [Lookup].[ShipmentQuantityUnit] SU ON SU.Id = S.Units
	INNER JOIN	[Lookup].[NotificationStatus] NS  ON [NS].[Id] = [NA].[Status]
	INNER JOIN	[ImportNotification].[WasteOperation] WO ON WO.ImportNotificationId = N.Id
    LEFT JOIN [ImportNotification].[WasteType] WasteType
        INNER JOIN [ImportNotification].[WasteCode] WasteCode ON WasteType.Id = WasteCode.WasteTypeId
        LEFT JOIN [Lookup].[WasteCode] WasteCodeInfo ON WasteCode.WasteCodeId = WasteCodeInfo.Id
    ON WasteType.ImportNotificationId = N.Id AND WasteCodeInfo.CodeType IN (1, 2)
    LEFT JOIN [ImportNotification].[Consent] C ON C.NotificationId = N.Id
    LEFT JOIN [ImportNotification].[Consultation] CON 
        INNER JOIN [Lookup].[LocalArea] LA ON CON.LocalAreaId = LA.Id
    ON CON.NotificationId = N.Id
    LEFT JOIN [ImportNotification].[InterimStatus] InS ON	[N].[Id] = [InS].[ImportNotificationId]
    LEFT JOIN [ImportNotification].[Movement] M ON M.[NotificationId] = N.Id
    LEFT JOIN [ImportNotification].[MovementReceipt] MR ON MR.MovementId = M.Id
    LEFT JOIN [ImportNotification].[MovementOperationReceipt] MOR ON MOR.MovementId = M.Id
	LEFT JOIN [Reports].[TransitStatesConcat] ETS on ETS.NotificationId = N.Id
	LEFT JOIN [ImportNotification].[FinancialGuarantee] FG ON FG.[ImportNotificationId] = N.[Id] 
    LEFT JOIN [ImportNotification].[Objection] O ON NA.NotificationApplicationId = O.NotificationId
GO