IF OBJECT_ID('[Notification].[uspNotificationProgress]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspNotificationProgress] AS SET NOCOUNT ON;')
GO
 
ALTER PROCEDURE [Notification].[uspNotificationProgress]
    @NotificationId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT
        N.[Id],
        N.[NotificationType],
        N.[CompetentAuthority],
        N.[NotificationNumber],
        FC.[AllFacilitiesPreconsented] AS [IsPreconsentedRecoveryFacility],
        N.[ReasonForExport],
        N.[HasSpecialHandlingRequirements],
        MOT.[MeansOfTransport],
        N.[IsRecoveryPercentageDataProvidedByImporter],
        N.[IsWasteGenerationProcessAttached],
        N.[WasteGenerationProcess],
        E.[Id] AS [ExporterId],
        I.[Id] AS [ImporterId],
        CC.[Id] AS [CarrierId],
        OC.[Id] AS [OperationCodesId],
        T.[Id] AS [TechnologyEmployedId],
        PI.[Id] AS [PackagingInfoId],
        PC.[Id] AS [PhysicalCharacteristicsId],
        R.[Id] AS [RecoveryInfoId],
        R.[PercentageRecoverable],
        D.[Id] AS [DisposalInfoId],
        S.[Id] AS [ShipmentInfoId],
        WT.[Id] AS [WasteTypeId],
        WT.[OtherWasteTypeDescription],
        WT.[HasAnnex] AS [WasteTypeHasAnnex],
        WA.[Id] AS [WasteAdditionalInformationId],
        SE.[Id] AS [StateOfExportId],
        SI.[Id] AS [StateOfImportId],
        TS.[Id] AS [TransitStateId]
    FROM
        [Notification].[Notification] N
    
        LEFT JOIN (SELECT TOP (1) NotificationId, Id FROM [Notification].[Exporter] WHERE NotificationId = @NotificationId) AS E 
        ON N.Id = E.NotificationId

        LEFT JOIN (SELECT TOP (1) NotificationId, Id FROM [Notification].[Importer] WHERE NotificationId = @NotificationId) AS I
        ON N.Id = I.NotificationId

        LEFT JOIN (SELECT TOP (1) NotificationId, Id FROM [Notification].[OperationCodes] WHERE NotificationId = @NotificationId) AS OC 
        ON N.Id = OC.NotificationId

        LEFT JOIN (SELECT TOP (1) NotificationId, Id FROM [Notification].[TechnologyEmployed] WHERE NotificationId = @NotificationId) AS T 
        ON N.Id = T.NotificationId

        LEFT JOIN (SELECT TOP (1) NotificationId, Id FROM [Notification].[PackagingInfo] WHERE NotificationId = @NotificationId) AS PI 
        ON N.Id = PI.NotificationId

        LEFT JOIN (SELECT TOP (1) NotificationId, Id FROM [Notification].[PhysicalCharacteristicsInfo] WHERE NotificationId = @NotificationId) AS PC 
        ON N.Id = PC.NotificationId

        LEFT JOIN (SELECT TOP (1) NotificationId, Id, PercentageRecoverable FROM [Notification].[RecoveryInfo] WHERE NotificationId = @NotificationId) AS R 
        ON N.Id = R.NotificationId

        LEFT JOIN (SELECT TOP (1) NotificationId, Id FROM [Notification].[DisposalInfo] WHERE NotificationId = @NotificationId) AS D 
        ON N.Id = D.NotificationId

        LEFT JOIN (SELECT TOP (1) NotificationId, Id FROM [Notification].[ShipmentInfo] WHERE NotificationId = @NotificationId) AS S 
        ON N.Id = S.NotificationId

        LEFT JOIN (SELECT TOP (1) NotificationId, Id, OtherWasteTypeDescription, HasAnnex FROM [Notification].[WasteType] WHERE NotificationId = @NotificationId) AS WT 
        ON N.Id = WT.NotificationId

        LEFT JOIN (SELECT TOP (1) NotificationId, WAI.Id FROM [Notification].[WasteType] WT INNER JOIN [Notification].[WasteAdditionalInformation] WAI ON WT.Id = WAI.WasteTypeId WHERE NotificationId = @NotificationId) AS WA 
        ON N.Id = WA.NotificationId

        LEFT JOIN (SELECT TOP (1) TR.NotificationId, SE.Id FROM [Notification].[StateOfExport] SE INNER JOIN [Notification].[TransportRoute] TR ON TR.[Id] = SE.[TransportRouteId] WHERE TR.NotificationId = @NotificationId) AS SE 
        ON N.Id = SE.NotificationId 

        LEFT JOIN (SELECT TOP (1) TR.NotificationId, SI.Id FROM [Notification].[StateOfImport] SI INNER JOIN [Notification].[TransportRoute] TR ON TR.[Id] = SI.[TransportRouteId] WHERE TR.NotificationId = @NotificationId) AS SI 
        ON N.Id = SI.NotificationId

        LEFT JOIN (SELECT TOP (1) TR.NotificationId, TS.Id FROM [Notification].[TransitState] TS INNER JOIN [Notification].[TransportRoute] TR ON TR.[Id] = TS.[TransportRouteId] WHERE TR.NotificationId = @NotificationId) AS TS 
        ON N.Id = TS.NotificationId

        LEFT JOIN [Notification].[FacilityCollection] FC ON N.Id = FC.NotificationId

        LEFT JOIN (SELECT TOP(1) CC.NotificationId, C.Id FROM [Notification].[CarrierCollection] CC INNER JOIN [Notification].[Carrier] C ON CC.[Id] = C.[CarrierCollectionId] WHERE CC.NotificationId = @NotificationId) AS CC
        ON N.Id = CC.NotificationId

        LEFT JOIN [Notification].[MeansOfTransport] MOT ON N.Id = MOT.NotificationId
    WHERE
        N.Id = @NotificationId;

    SELECT DISTINCT
        P.Id,
        P.IsSiteOfExport
    FROM
        [Notification].[Notification] N

        INNER JOIN [Notification].[ProducerCollection] PC
        ON N.Id = PC.NotificationId

        LEFT JOIN [Notification].[Producer] P
        ON PC.Id = P.ProducerCollectionId
    WHERE
        N.Id = @NotificationId;

    SELECT DISTINCT
        F.Id,
        F.IsActualSiteOfTreatment
    FROM
        [Notification].[Notification] N
        
        INNER JOIN [Notification].[FacilityCollection] FC
        ON FC.NotificationId = N.Id

        LEFT JOIN [Notification].[Facility] F
        ON FC.Id = F.FacilityCollectionId
    WHERE
        N.Id = @NotificationId;

    SELECT DISTINCT
        WCI.CodeType
    FROM
        [Notification].[Notification] N

        LEFT JOIN [Notification].[WasteCodeInfo] AS WCI
        ON N.Id = WCI.NotificationId
    WHERE
        N.Id = @NotificationId;

    SELECT DISTINCT
        ImportCountry.IsEuropeanUnionMember AS ImportIsEuMember,
        ExportCountry.IsEuropeanUnionMember AS ExportIsEuMember,
        TransitCountry.IsEuropeanUnionMember AS TransitIsEuMember,
        ECO.Id AS EntryCustomsOfficeId,
        XCO.Id AS ExitCustomsOfficeId,
        EECS.[Entry] AS IsEntryCustomsOfficeRequired,
        EECS.[Exit] AS IsExitCustomsOfficeRequired
    FROM
        [Notification].[TransportRoute] TR
        LEFT JOIN [Notification].[EntryCustomsOffice] ECO on TR.Id = ECO.TransportRouteId
        LEFT JOIN [Notification].[ExitCustomsOffice] XCO on TR.Id = XCO.TransportRouteId
        LEFT JOIN [Notification].[EntryExitCustomsSelection] EECS on TR.Id = EECS.TransportRouteId
        LEFT JOIN [Notification].[StateOfExport] SE on TR.Id = SE.TransportRouteId 
        LEFT JOIN [Notification].[StateOfImport] SI on TR.Id = SI.TransportRouteId
        LEFT JOIN [Notification].[TransitState] TS on TR.Id = TS.TransportRouteId
        LEFT JOIN [Lookup].Country ImportCountry on SI.CountryId = ImportCountry.Id
        LEFT JOIN [Lookup].Country ExportCountry on SE.CountryId = ExportCountry.Id
        LEFT JOIN [Lookup].Country TransitCountry on TS.CountryId = TransitCountry.Id
    WHERE
        TR.NotificationId = @NotificationId;

END
GO