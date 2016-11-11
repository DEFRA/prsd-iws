IF OBJECT_ID('[Reports].[FreedomOfInformation]') IS NULL
    EXEC('CREATE VIEW [Reports].[FreedomOfInformation] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[FreedomOfInformation]
AS
    SELECT 
        N.[CompetentAuthorityId],
        O.[Exporter] AS [NotifierName],
        O.[ExporterAddress] AS [NotifierAddress],
        O.[Producer] AS [ProducerName],
        O.[ProducerAddress],
        TR.[ExitPoint] AS [PointOfExport],
        WT.ChemicalCompositionTypeId,
        WT.[ChemicalCompositionType] AS [NameOfWaste],
        WC_EWC.[Code] AS [EWC],
        WC_Y.[Code] AS [YCode],
        O.[Facility] AS [FacilityName],
        O.[FacilityAddress],
        M.[QuantityReceived],
        M.[QuantityReceivedUnit],
        M.[ReceivedDate]
    FROM
        [Reports].[Notification] N
        INNER JOIN [Reports].[NotificationOrganisations] O ON N.Id = O.Id
        INNER JOIN [Reports].[WasteType] WT ON N.Id = WT.NotificationId
        INNER JOIN [Reports].[Movements] M ON N.Id = M.NotificationId
        INNER JOIN [Reports].[TransportRoute] TR ON N.Id = TR.NotificationId
        LEFT JOIN [Reports].[WasteCodes] WC_EWC ON N.Id = WC_EWC.NotificationId AND WC_EWC.CodeType = 3
        LEFT JOIN [Reports].[WasteCodes] WC_Y ON N.Id = WC_Y.NotificationId AND WC_Y.CodeType = 4
    WHERE
        N.[ImportOrExport] = 'Export'