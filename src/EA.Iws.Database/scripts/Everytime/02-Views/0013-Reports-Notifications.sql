IF OBJECT_ID('[Reports].[Notification]') IS NULL
    EXEC('CREATE VIEW [Reports].[Notification] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[Notification]
AS
    SELECT
        N.[Id] AS Id,
        REPLACE(N.[NotificationNumber], ' ', '') AS [NotificationNumber],
        N.[UserId],
        NT.[Description] AS [Type],
        NT.Id AS [TypeId],
        CA.[UnitedKingdomCountry] AS [CompetentAuthorityCountry],
        CASE 
            WHEN CA.UnitedKingdomCountry = 'England' THEN 'EA'
            WHEN CA.UnitedKingdomCountry = 'Scotland' THEN 'SEPA'			
            WHEN CA.UnitedKingdomCountry = 'Northern Ireland' THEN 'NIEA'
            WHEN CA.UnitedKingdomCountry = 'Wales' THEN 'NRW'
        END AS [CompetentAuthority],
        CA.Id AS [CompetentAuthorityId],
        NS.[Description] AS [Status],
        LA.[Name] AS [LocalArea],
        SI.[FirstDate] AS [IntendedFrom],
        SI.[LastDate] AS [IntendedTo],
        SI.[Quantity] AS [IntendedQuantity],
        SI.[NumberOfShipments] AS [NumberOfShipments],
        U.[Description] AS [Units],
        U.[Id] AS [UnitsId],
        C.[From] AS [ConsentFrom],
        C.[To] AS [ConsentTo],
        FC.[AllFacilitiesPreconsented] AS [Preconsented],
        'Export' AS [ImportOrExport],
        (SELECT Price FROM [Notification].[GetPricingInfo](N.Id)) AS [Charge],
        FC.[IsInterim]

    FROM		[Notification].[Notification] AS N

    INNER JOIN	[Lookup].[NotificationType] AS NT
    ON			[NT].[Id] = [N].[NotificationType]

    INNER JOIN	[Lookup].[UnitedKingdomCompetentAuthority] AS CA
    ON			[CA].[Id] = [N].[CompetentAuthority]

    INNER JOIN	[Notification].[NotificationAssessment] AS NA 
    ON			[NA].[NotificationApplicationId] = [N].[Id]

    INNER JOIN	[Lookup].[NotificationStatus] AS NS
    ON			[NS].[Id] = [NA].[Status]
    
    LEFT JOIN	[Notification].[Consultation] AS CON
    ON			[CON].[NotificationId] = [N].[Id]

    LEFT JOIN	[Lookup].[LocalArea] AS LA
    ON			[CON].[LocalAreaId] = [LA].[Id]

    INNER JOIN	[Notification].[ShipmentInfo] AS SI
    ON			[SI].[NotificationId] = [N].[Id]

    INNER JOIN	[Lookup].[ShipmentQuantityUnit] AS U
    ON			[SI].[Units] = [U].[Id]

    LEFT JOIN	[Notification].[Consent] AS C
    ON			[N].[Id] = [C].[NotificationApplicationId]

    LEFT JOIN	[Notification].[FacilityCollection] FC
    ON			N.[Id] = FC.[NotificationId]

    UNION ALL

    SELECT 
        N.[Id] AS Id,
        REPLACE(N.[NotificationNumber], ' ', '') AS [NotificationNumber],
        NULL AS [UserId],
        NT.[Description] AS [Type],
        NT.[Id] AS [TypeId],
        CA.[UnitedKingdomCountry] AS [CompetentAuthorityCountry],
        CASE 
            WHEN CA.UnitedKingdomCountry = 'England' THEN 'EA'
            WHEN CA.UnitedKingdomCountry = 'Scotland' THEN 'SEPA'			
            WHEN CA.UnitedKingdomCountry = 'Northern Ireland' THEN 'NIEA'
            WHEN CA.UnitedKingdomCountry = 'Wales' THEN 'NRW'
        END AS [CompetentAuthority],
        CA.[Id] AS [CompetentAuthorityId],
        NS.[Description] AS [Status],		
        LA.[Name] AS [LocalArea],	
        S.[FirstDate] AS [IntendedFrom],
        S.[LastDate] AS [IntendedTo],
        S.[Quantity] AS [IntendedQuantity],
        S.[NumberOfShipments] AS [NumberOfShipments],
        U.[Description] AS [Units],
        U.[Id] AS [UnitsId],
        C.[From] AS [ConsentFrom],
        C.[To] AS [ConsentTo],
        FC.[AllFacilitiesPreconsented] AS [Preconsented],
        'Import' AS [ImportOrExport],
        (SELECT Price FROM [Notification].[GetPricingInfo](N.Id)) AS [Charge],
        InS.[IsInterim]

    FROM [ImportNotification].[Notification] AS N

    INNER JOIN	[Lookup].[NotificationType] AS NT
    ON			[NT].[Id] = [N].[NotificationType]

    INNER JOIN  [ImportNotification].[NotificationAssessment] NA
    ON			[NA].NotificationApplicationId = [N].Id

    INNER JOIN  [Lookup].[ImportNotificationStatus] NS
    ON			[NA].[Status] = [NS].[Id]

    LEFT JOIN	[ImportNotification].[Consultation] AS CON
    ON			[CON].[NotificationId] = [N].[Id]

    LEFT JOIN	[Lookup].[LocalArea] AS LA
    ON			[CON].[LocalAreaId] = [LA].[Id]

    INNER JOIN	[Lookup].[UnitedKingdomCompetentAuthority] AS [CA]
    ON			[CA].[Id] = [N].[CompetentAuthority]

    LEFT JOIN	[ImportNotification].[Shipment] AS S
    ON			[N].[Id] = [S].[ImportNotificationId]

    LEFT JOIN	[Lookup].[ShipmentQuantityUnit] AS U
    ON			[S].[Units] = [U].[Id]

    LEFT JOIN	[ImportNotification].[Consent] AS C
    ON			[N].[Id] = [C].[NotificationId]

    LEFT JOIN   [ImportNotification].[FacilityCollection] AS FC
    ON			[N].[Id] = [FC].[ImportNotificationId]

    LEFT JOIN   [ImportNotification].[InterimStatus] InS 
    ON			[N].[Id] = [InS].[ImportNotificationId]
GO