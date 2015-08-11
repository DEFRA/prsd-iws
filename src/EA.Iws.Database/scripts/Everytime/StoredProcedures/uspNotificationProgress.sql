IF OBJECT_ID('[Notification].[uspNotificationProgress]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspNotificationProgress] AS SET NOCOUNT ON;')
GO
 
ALTER PROCEDURE [Notification].[uspNotificationProgress]
    @NotificationId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @WasteCodes TABLE(CodeType INT);
    DECLARE @IsImportInEu BIT;
    DECLARE @IsExportInEu BIT;
    DECLARE @IsTransitAllEu BIT;
    DECLARE @HasEntryCO BIT;
    DECLARE @HasExitCO BIT;
    DECLARE @HasCustomsOffice BIT = 0;
    DECLARE @HasWasteCodes BIT = 0;

    INSERT INTO @WasteCodes 
    SELECT 
        DISTINCT wc.CodeType 
    FROM
        [Business].WasteCodeInfo wci
        INNER JOIN [Lookup].WasteCode wc ON wci.WasteCodeId = wc.Id
    WHERE 
        wci.NotificationId = @NotificationId;

    IF
        (SELECT COUNT(CodeType) FROM @WasteCodes WHERE CodeType IN (1, 2)) = 1 AND 
        (SELECT COUNT(CodeType) FROM @WasteCodes WHERE CodeType IN (3, 4, 5, 6, 7, 8, 11)) = 7
    BEGIN
        SET @HasWasteCodes = 1;
    END;

    SELECT DISTINCT
        @IsImportInEu = ImportCountry.IsEuropeanUnionMember
        ,@IsExportInEu = ExportCountry.IsEuropeanUnionMember
        ,@IsTransitAllEu = (
            SELECT
                MIN(case when c3.IsEuropeanUnionMember = 1 then 1 else 0 end)
            FROM
                [Notification].[Notification] N2
                LEFT JOIN [Notification].[TransitState] ts on N2.Id = ts.NotificationId
                INNER JOIN [Lookup].Country c3 on ts.CountryId = c3.Id
            WHERE
                N2.Id = N.Id
         )
         ,@HasEntryCO = CASE WHEN ECO.Id IS NULL THEN 0 ELSE 1 END
         ,@HasExitCO = CASE WHEN XCO.Id IS NULL THEN 0 ELSE 1 END
    FROM
        [Notification].[Notification] N
        LEFT JOIN [Notification].[EntryCustomsOffice] ECO on N.Id = ECO.NotificationId
        LEFT JOIN [Notification].[ExitCustomsOffice] XCO on N.Id = XCO.NotificationId
        LEFT JOIN [Notification].[StateOfExport] SE on N.Id = SE.NotificationId 
        LEFT JOIN [Notification].[StateOfImport] SI on N.Id = SI.NotificationId
        LEFT JOIN [Lookup].Country ImportCountry on SI.CountryId = ImportCountry.Id
        LEFT JOIN [Lookup].Country ExportCountry on SE.CountryId = ExportCountry.Id
    WHERE 
        N.Id = @NotificationId;

    IF @IsImportInEu = 1 AND @IsExportInEu = 1 AND @IsTransitAllEu = 1
    BEGIN
        SET @HasCustomsOffice = 1;
    END
    ELSE IF @IsImportInEu = 0 AND @IsExportInEu = 1 AND @IsTransitAllEu = 1 AND @HasExitCO = 1
    BEGIN
        SET @HasCustomsOffice = 1;
    END
    ELSE IF @IsImportInEu = 0 AND @IsExportInEu = 1 AND @IsTransitAllEu = 0 AND @HasExitCO = 1
    BEGIN
        SET @HasCustomsOffice = 1;
    END
    ELSE IF @IsImportInEu = 1 AND @IsExportInEu = 1 AND @IsTransitAllEu = 0 AND @HasExitCO = 1 AND @HasEntryCO = 1
    BEGIN
        SET @HasCustomsOffice = 1;
    END

    /*
    Select completion status as boolean (bit) for each entity type.
    */
    SELECT DISTINCT 
        [N].[Id],
        [N].[NotificationType],
        [N].[CompetentAuthority],
        [N].[NotificationNumber],
        CAST(CASE WHEN [N].[NotificationType] = 1 AND [IsPreconsentedRecoveryFacility] IS NULL THEN 0 ELSE 1 END AS BIT) AS HasPreconsentedInformation,
        CAST(CASE WHEN [ReasonForExport] IS NULL THEN 0 ELSE 1 END AS BIT) AS HasReasonForExport,
        CAST(CASE WHEN [HasSpecialHandlingRequirements] IS NULL THEN 0 ELSE 1 END AS BIT) AS HasSpecialHandlingRequirements,
        CAST(CASE WHEN [MeansOfTransport] IS NULL THEN 0 ELSE 1 END AS BIT) AS HasMeansOfTransport,
        CAST(CASE 
            WHEN [IsRecoveryPercentageDataProvidedByImporter] IS NULL THEN 
                CASE 
                    WHEN [PercentageRecoverable] IS NULL THEN 0 
                    ELSE 1 
                END
            ELSE 1
         END AS BIT) AS HasRecoveryData,
        CAST(CASE WHEN [MethodOfDisposal] IS NULL THEN 0 ELSE 1 END AS BIT) AS HasMethodOfDisposal,
        CAST(CASE 
            WHEN [IsWasteGenerationProcessAttached] = 1 THEN 1 
            ELSE 
                CASE 
                    WHEN [WasteGenerationProcess] IS NULL THEN 0 
                    ELSE 1 
                END 
         END AS BIT) AS HasWasteGenerationProcess,
        CAST(CASE WHEN E.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasExporter,
        CAST(CASE WHEN P.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasProducer,
        CAST(ISNULL((SELECT MAX(CASE WHEN IsSiteOfExport = 1 THEN 1 ELSE 0 END) FROM [Business].[Producer] WHERE NotificationId = N.Id), 0) AS BIT) AS HasSiteOfExport,
        CAST(CASE WHEN I.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasImporter,
        CAST(CASE WHEN F.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasFacility,
        CAST(ISNULL((SELECT MAX(CASE WHEN IsActualSiteOfTreatment = 1 THEN 1 ELSE 0 END) FROM [Business].[Facility] WHERE NotificationId = N.Id), 0) AS BIT) AS HasActualSiteOfTreatment,
        CAST(CASE WHEN C.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasCarrier,
        CAST(CASE WHEN OC.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasOperationCodes,
        CAST(CASE WHEN T.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasTechnologyEmployed,
        CAST(CASE WHEN PI.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasPackagingInfo,
        CAST(CASE WHEN PC.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasPhysicalCharacteristics,
        CAST(CASE 
            WHEN N.IsRecoveryPercentageDataProvidedByImporter = 1 THEN 1
            ELSE
                CASE
                    WHEN R.Id IS NULL THEN 0 
                    ELSE 1
                END 
         END AS BIT) AS HasRecoveryInfo,
        CAST(CASE WHEN S.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasShipmentInfo,
        CAST(
            CASE 
                WHEN WT.Id IS NULL then 0 
                ELSE 
                    CASE WHEN WT.OtherWasteTypeDescription IS NOT NULL
                        OR WT.HasAnnex = 1
                        OR WA.Id is not null
                        THEN 1
                        ELSE 0
                    END 
            END 
            AS BIT) AS HasWasteType,
        CAST(CASE WHEN SE.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasStateOfExport,
        CAST(CASE WHEN SI.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasStateOfImport,
        CAST(CASE WHEN TS.Id IS NULL THEN 0 ELSE 1 END AS BIT) AS HasTransitState,
        @HasCustomsOffice AS HasCustomsOffice,
        @HasWasteCodes AS HasWasteCodes
    FROM 
        [Notification].[Notification] N

        LEFT JOIN [Business].[Exporter] AS E 
        ON N.Id = E.NotificationId

        LEFT JOIN [Business].[Producer] AS P 
        ON N.Id = P.NotificationId

        LEFT JOIN [Business].[Importer] AS I
        ON N.Id = I.NotificationId

        LEFT JOIN [Business].[Facility] AS F 
        ON N.Id = F.NotificationId

        LEFT JOIN [Business].[Carrier] AS C 
        ON N.Id = C.NotificationId

        LEFT JOIN [Business].[OperationCodes] AS OC 
        ON N.Id = OC.NotificationId

        LEFT JOIN [Notification].[TechnologyEmployed] AS T 
        ON N.Id = T.NotificationId

        LEFT JOIN [Business].[PackagingInfo] AS PI 
        ON N.Id = PI.NotificationId

        LEFT JOIN [Business].[PhysicalCharacteristicsInfo] AS PC 
        ON N.Id = PC.NotificationId

        LEFT JOIN [Business].[RecoveryInfo] AS R 
        ON N.Id = R.NotificationId

        LEFT JOIN [Business].[ShipmentInfo] AS S 
        ON N.Id = S.NotificationId

        LEFT JOIN [Business].[WasteCodeInfo] AS WCI 
        ON N.Id = WCI.NotificationId

        LEFT JOIN [Lookup].[WasteCode] AS WC 
        ON WCI.WasteCodeId = WC.Id

        LEFT JOIN [Business].[WasteType] AS WT 
        ON N.Id = WT.NotificationId

        LEFT JOIN [Business].[WasteAdditionalInformation] AS WA 
        ON WT.Id = WA.WasteTypeId

        LEFT JOIN [Notification].[StateOfExport] AS SE 
        ON N.Id = SE.NotificationId 

        LEFT JOIN [Notification].[StateOfImport] AS SI 
        ON N.Id = SI.NotificationId

        LEFT JOIN [Notification].[TransitState] AS TS 
        ON N.Id = TS.NotificationId
    WHERE 
        N.Id = @NotificationId;
END
GO