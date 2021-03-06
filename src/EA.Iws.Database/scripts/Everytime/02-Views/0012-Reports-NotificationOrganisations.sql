IF OBJECT_ID('[Reports].[NotificationOrganisations]') IS NULL
    EXEC('CREATE VIEW [Reports].[NotificationOrganisations] AS SELECT 1 AS [NOTHING];')
GO

/*
*	Returns the organisation details for each of the businesses involved in an export notification.
*	Facility: Takes actual site if available, any facility if not, or null where none available
*	Producer: Takes site of export if available, any producer if not, or null where none available
*/
ALTER VIEW [Reports].[NotificationOrganisations]
AS
    SELECT	N.[Id],
            REPLACE(N.[NotificationNumber], ' ', '') AS [NotificationNumber],
            E.[Name] AS [Exporter],
            E.[FullName] AS [ExporterContactName],
            E.[RegistrationNumber] AS [ExporterRegistrationNumber],
            [Reports].[ConcatenateAddress](E.[Address1], E.[Address2], E.[TownOrCity], E.[PostalCode], E.[Region], E.[Country]) AS [ExporterAddress],
            E.[PostalCode] AS [ExporterPostalCode],
            I.[Name] AS [Importer],
            I.[FullName] AS [ImporterContactName],
            I.[RegistrationNumber] AS [ImporterRegistrationNumber],
            [Reports].[ConcatenateAddress](I.[Address1], I.[Address2], I.[TownOrCity], I.[PostalCode], I.[Region], I.[Country]) AS [ImporterAddress],
            I.[PostalCode] AS [ImporterPostalCode],
            P.[Name] AS [Producer],
            P.[FullName] AS [ProducerContactName],
            P.[RegistrationNumber] AS [ProducerRegistrationNumber],
            [Reports].[ConcatenateAddress](P.[Address1], P.[Address2], P.[TownOrCity], P.[PostalCode], P.[Region], P.[Country]) AS [ProducerAddress],
            P.[PostalCode] AS [ProducerPostalCode],
            CASE WHEN P2.NumberOfProducers > 1 THEN 1 ELSE 0 END AS [HasMultipleProducers],
            F.[Name] AS [Facility],
            F.[FullName] AS [FacilityContactName],
            F.[RegistrationNumber] AS [FacilityRegistrationNumber],
            [Reports].[ConcatenateAddress](F.[Address1], F.[Address2], F.[TownOrCity], F.[PostalCode], F.[Region], F.[Country]) AS [FacilityAddress],
            F.[PostalCode] AS [FacilityPostalCode],
            CASE WHEN F2.NumberOfFacilities > 1 THEN 1 ELSE 0 END AS [HasMultipleFacilities]

    FROM [Notification].[Notification] AS N

    LEFT JOIN [Notification].[Exporter] AS E
    ON E.[NotificationId] = N.[Id]

    LEFT JOIN [Notification].[Importer] AS I
    ON I.[NotificationId] = N.[Id]

    LEFT JOIN	[Notification].[Facility] AS F
    ON			F.Id = 
                (
                    SELECT TOP 1 F1.Id

                    FROM		[Notification].[FacilityCollection] AS FC

                    INNER JOIN	[Notification].[Facility] AS F1
                    ON			FC.Id = F1.FacilityCollectionId

                    WHERE		NotificationId = N.Id
                    ORDER BY	F1.IsActualSiteOfTreatment DESC
                )

    LEFT JOIN (
        SELECT FC.NotificationId, COUNT(F1.Id) AS NumberOfFacilities

        FROM		[Notification].[Facility] AS F1

        INNER JOIN	[Notification].[FacilityCollection] AS FC
        ON			F1.FacilityCollectionId = FC.Id

        GROUP BY FC.NotificationId

    ) F2 ON F2.NotificationId = N.Id

    LEFT JOIN	[Notification].[Producer] AS P
    ON			P.Id = 
                (
                    SELECT TOP 1 P1.Id

                    FROM		[Notification].[ProducerCollection] AS PC

                    INNER JOIN [Notification].[Producer] AS P1
                    ON		   PC.Id = P1.ProducerCollectionId

                    WHERE		PC.NotificationId = N.Id
                    ORDER BY	P1.[IsSiteOfExport] DESC
                )

    LEFT JOIN (
        SELECT PC.NotificationId, COUNT(P1.Id) AS [NumberOfProducers]

        FROM		[Notification].[Producer] AS P1

        INNER JOIN	[Notification].[ProducerCollection] AS PC
        ON			P1.ProducerCollectionId = PC.Id

        GROUP BY PC.NotificationId

    ) P2 ON P2.NotificationId = N.Id

    UNION ALL

    SELECT	N.[Id],
            REPLACE(N.[NotificationNumber], ' ', '') AS [NotificationNumber],
            E.[Name] AS [Exporter],
            E.[ContactName] AS [ExporterContactName],
            NULL AS [ExporterRegistrationNumber],
            [Reports].[ConcatenateAddress](E.[Address1], E.[Address2], E.[TownOrCity], E.[PostalCode], NULL, C_E.[Name]) AS [ExporterAddress],
            E.[PostalCode] AS [ExporterPostalCode],
            I.[Name] AS [Importer],
            I.[ContactName] AS [ImporterContactName],
            I.[RegistrationNumber] AS [ImporterRegistrationNumber],
            [Reports].[ConcatenateAddress](I.[Address1], I.[Address2], I.[TownOrCity], I.[PostalCode], NULL, C_I.[Name]) AS [ImporterAddress],
            I.[PostalCode] AS [ImporterPostalCode],
            P.[Name] AS [Producer],
            P.[ContactName] AS [ProducerContactName],
            NULL AS [ProducerRegistrationNumber],
            [Reports].[ConcatenateAddress](P.[Address1], P.[Address2], P.[TownOrCity], P.[PostalCode], NULL, C_P.[Name]) AS [ProducerAddress],
            P.[PostalCode] AS [ProducerPostalCode],
            CASE WHEN P.IsOnlyProducer = 1 THEN 0 ELSE 1 END AS [HasMultipleProducers],
            F.[Name] AS [Facility],
            F.[ContactName] AS [FacilityContactName],
            F.[RegistrationNumber] AS [FacilityRegistrationNumber],
            [Reports].[ConcatenateAddress](F.[Address1], F.[Address2], F.[TownOrCity], F.[PostalCode], NULL, C_F.[Name]) AS [FacilityAddress],
            F.[PostalCode] AS [FacilityPostalCode],
            CASE WHEN F2.NumberOfFacilities > 1 THEN 1 ELSE 0 END AS [HasMultipleFacilities]

    FROM		[ImportNotification].[Notification] AS N

    LEFT JOIN	[ImportNotification].[Exporter] AS E
    ON			E.[ImportNotificationId] = N.[Id]

    LEFT JOIN	[Lookup].[Country] AS C_E
    ON			E.[CountryId] = C_E.[Id]

    LEFT JOIN	[ImportNotification].[Importer] AS I
    ON			I.[ImportNotificationId] = N.[Id]

    LEFT JOIN	[Lookup].[Country] AS C_I
    ON			I.[CountryId] = C_I.[Id]

    LEFT JOIN	[ImportNotification].[Facility] AS F
    ON			F.Id = 
                (
                    SELECT TOP 1 F1.Id

                    FROM		[ImportNotification].[FacilityCollection] AS FC

                    INNER JOIN	[ImportNotification].[Facility] AS F1
                    ON			FC.Id = F1.FacilityCollectionId

                    WHERE		FC.ImportNotificationId = N.Id
                    ORDER BY	F1.IsActualSiteOfTreatment DESC
                )

    LEFT JOIN (
        SELECT FC.ImportNotificationId, COUNT(F1.Id) AS NumberOfFacilities

        FROM		[ImportNotification].[Facility] AS F1

        INNER JOIN	[ImportNotification].[FacilityCollection] AS FC
        ON			F1.FacilityCollectionId = FC.Id

        GROUP BY FC.ImportNotificationId

    ) F2 ON F2.ImportNotificationId = N.Id
        
    LEFT JOIN	[Lookup].[Country] AS C_F
    ON			F.[CountryId] = C_F.[Id]
    
    LEFT JOIN	[ImportNotification].[Producer] AS P
    ON			P.Id = 
                (
                    SELECT TOP 1 Id
                    FROM		[ImportNotification].[Producer]
                    WHERE		ImportNotificationId = N.Id
                )
                    
    LEFT JOIN	[Lookup].[Country] AS C_P
    ON			P.[CountryId] = C_P.[Id]
GO
