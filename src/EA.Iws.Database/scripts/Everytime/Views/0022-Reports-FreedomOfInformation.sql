IF OBJECT_ID('[Reports].[FreedomOfInformation]') IS NULL
    EXEC('CREATE VIEW [Reports].[FreedomOfInformation] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[FreedomOfInformation]
AS
    SELECT
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
        O.[Importer] AS [ImporterName],
        O.[ImporterAddress],
        O.[Facility] AS [FacilityName],
        O.[FacilityAddress],
        (SELECT	SUM(
            CASE WHEN [QuantityReceivedUnitId] IN (1, 2) -- Tonnes / Cubic Metres
                THEN COALESCE([QuantityReceived], 0)
            ELSE 
                COALESCE([QuantityReceived] * 1000, 0) -- Convert to Tonnes / Cubic Metres
            END
            ) 
            FROM [Reports].[Movements]
            WHERE N.Id = NotificationId
        ) AS [QuantityReceived],
        CASE WHEN N.[UnitsId] IN (1, 2) -- Due to conversion units will only be Tonnes / Cubic Metres
            THEN N.[Units] 
        WHEN N.[UnitsId] = 3 THEN 'Tonnes'
        WHEN N.[UnitsId] = 4 THEN 'Cubic Metres'
        END AS [QuantityReceivedUnit],
        N.[IntendedQuantity],
        N.[Units] AS [IntendedQuantityUnit],
        NA.[ConsentFrom],
        NA.[ConsentTo]
    FROM
        [Reports].[Notification] N
        INNER JOIN [Reports].[NotificationOrganisations] O ON N.Id = O.Id
        INNER JOIN [Reports].[NotificationAssessment] NA ON N.Id = NA.NotificationId
        INNER JOIN [Reports].[WasteType] WT ON N.Id = WT.NotificationId
        INNER JOIN [Reports].[TransportRoute] TR ON N.Id = TR.NotificationId
    WHERE
        N.[ImportOrExport] = 'Export'

GO