IF OBJECT_ID('[Reports].[FreedomOfInformation]') IS NULL
    EXEC('CREATE VIEW [Reports].[FreedomOfInformation] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[FreedomOfInformation]
AS
    SELECT
		N.[Id],
        N.[NotificationNumber],
        NA.[ReceivedDate],
        N.[CompetentAuthorityId],
        O.[Exporter] AS [NotifierName],
        O.[ExporterAddress] AS [NotifierAddress],
        O.[Producer] AS [ProducerName],
        O.[ProducerAddress],
        TR.[ExitPoint] AS [PointOfExport],
        TR.[EntryPoint] AS [PointOfEntry],
        TR.[ImportCountryName],
        WT.ChemicalCompositionTypeId,
        WT.[ChemicalCompositionType] AS [NameOfWaste],
        STUFF(( SELECT ', ' + WC.Code AS [text()]
                   FROM [Reports].[WasteCodes] WC
                   WHERE WC.NotificationId = N.Id AND WC.CodeType = 3
                   order by 1
                   FOR XML PATH('')
                 ), 1, 1, '' ) AS [EWC],
        STUFF(( SELECT ', ' + WC.Code AS [text()]
                   FROM [Reports].[WasteCodes] WC
                   WHERE WC.NotificationId = N.Id AND WC.CodeType = 4
                   order by 1
                   FOR XML PATH('')
                 ), 1, 1, '' ) AS [YCode],
        OP.OperationCodes,
        O.[Importer] AS [ImporterName],
        O.[ImporterAddress],
        O.[Facility] AS [FacilityName],
        O.[FacilityAddress],
        N.[IntendedQuantity],
        N.[Units] AS [IntendedQuantityUnit],
		N.[UnitsId] AS [IntendedQuantityUnitId],
        NA.[ConsentFrom],
        NA.[ConsentTo],
        N.[LocalArea]
    FROM
        [Reports].[Notification] N
        INNER JOIN [Reports].[NotificationOrganisations] O ON N.Id = O.Id
        INNER JOIN [Reports].[NotificationAssessment] NA ON N.Id = NA.NotificationId
        INNER JOIN [Reports].[WasteType] WT ON N.Id = WT.NotificationId
        INNER JOIN [Reports].[TransportRoute] TR ON N.Id = TR.NotificationId
        INNER JOIN [Reports].[OperationCodesConcat] OP ON N.Id = OP.NotificationId

GO