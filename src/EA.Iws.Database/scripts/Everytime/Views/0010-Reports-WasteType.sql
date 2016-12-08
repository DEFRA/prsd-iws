IF OBJECT_ID('[Reports].[WasteType]') IS NULL
    EXEC('CREATE VIEW [Reports].[WasteType] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[WasteType]
AS
    SELECT
        N.Id AS NotificationId,
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        CASE 
            WHEN WT.ChemicalCompositionType IN (1, 2) THEN CCT.Description
            WHEN WC.Code IS NOT NULL THEN WC.Code + ' ' + WC.Description
            WHEN WT.ChemicalCompositionType = 3 THEN WT.WoodTypeDescription
            WHEN WT.ChemicalCompositionType = 4 THEN WT.ChemicalCompositionName
        END AS Description,
        CCT.Id AS ChemicalCompositionTypeId,
        CCT.Description AS ChemicalCompositionType,
        WT.ChemicalCompositionDescription,
        N.HasSpecialHandlingRequirements,
        N.SpecialHandlingDetails,
        'Export' AS [ImportOrExport]
    
    FROM		[Notification].[Notification] AS N

    INNER JOIN	[Notification].[WasteType] AS WT 
    ON			[WT].[NotificationId] = [N].[Id]
    
    INNER JOIN	[Lookup].[ChemicalCompositionType] AS CCT 
    ON			[WT].[ChemicalCompositionType] = [CCT].[Id]

    LEFT JOIN	[Reports].[WasteCodes] WC 
    ON			[N].[Id] = [WC].[NotificationId] 
    AND			[WC].[CodeType] IN (1, 2)

    UNION

    SELECT
        N.Id AS NotificationId,
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        CASE 
            WHEN WT.ChemicalCompositionType IN (1, 2) THEN CCT.Description
            WHEN WC.Code IS NOT NULL THEN WC.Code + ' ' + WC.Description
            WHEN WT.ChemicalCompositionType IN (3, 4) THEN WT.Name
        END AS Description,
        CCT.Id AS ChemicalCompositionTypeId,
        CCT.Description AS ChemicalCompositionType,
        NULL AS ChemicalCompositionDescription,
        NULL AS HasSpecialHandlingRequirements,
        NULL AS SpecialHandlingDetails,
        'Import' AS [ImportOrExport]

    FROM		[ImportNotification].[Notification] AS N

    INNER JOIN	[ImportNotification].[WasteType] AS WT
    ON			[WT].[ImportNotificationId] = [N].[Id]

    INNER JOIN	[Lookup].[ChemicalCompositionType] AS CCT 
    ON			[WT].[ChemicalCompositionType] = [CCT].[Id]

    LEFT JOIN	[Reports].[WasteCodes] WC 
    ON			[N].[Id] = [WC].[NotificationId] 
    AND			[WC].[CodeType] IN (1, 2)
GO