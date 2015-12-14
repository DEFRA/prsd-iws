IF OBJECT_ID('[Reports].[Notification]') IS NULL
    EXEC('CREATE VIEW [Reports].[Notification] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[Notification]
AS
	SELECT
		N.[Id] AS Id,
		N.[NotificationNumber],
		NT.[Description] AS [Type],
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
		C.[From] AS [ConsentFrom],
		C.[To] AS [ConsentTo],
		'Export' AS [ImportOrExport]

	FROM		[Notification].[Notification] AS N

	INNER JOIN	[Lookup].[NotificationType] AS NT
	ON			[NT].[Id] = [N].[NotificationType]

	INNER JOIN	[Lookup].[UnitedKingdomCompetentAuthority] AS CA
	ON			[CA].[Id] = [N].[CompetentAuthority]

	INNER JOIN	[Notification].[NotificationAssessment] AS NA 
	ON			[NA].[NotificationApplicationId] = [N].[Id]

	INNER JOIN	[Lookup].[NotificationStatus] AS NS
	ON			[NS].[Id] = [NA].[Status]

	LEFT JOIN	[Lookup].[LocalArea] AS LA
	ON			[NA].[LocalAreaId] = [LA].[Id]

	INNER JOIN	[Notification].[ShipmentInfo] AS SI
	ON			[SI].[NotificationId] = [N].[Id]

	INNER JOIN	[Lookup].[ShipmentQuantityUnit] AS U
	ON			[SI].[Units] = [U].[Id]

	LEFT JOIN	[Notification].[Consent] AS C
	ON			[N].[Id] = [C].[NotificationApplicationId]

	UNION

	SELECT 
		N.[Id] AS Id,
		N.[NotificationNumber],
		NT.[Description] AS [Type],
		CA.[UnitedKingdomCountry] AS [CompetentAuthorityCountry],
		CASE 
			WHEN CA.UnitedKingdomCountry = 'England' THEN 'EA'
			WHEN CA.UnitedKingdomCountry = 'Scotland' THEN 'SEPA'			
			WHEN CA.UnitedKingdomCountry = 'Northern Ireland' THEN 'NIEA'
			WHEN CA.UnitedKingdomCountry = 'Wales' THEN 'NRW'
		END AS [CompetentAuthority],
		CA.[Id] AS [CompetentAuthorityId],
		'New' AS [Status],		-- TODO
		NULL AS [LocalArea],	-- TODO
		S.[FirstDate] AS [IntendedFrom],
		S.[LastDate] AS [IntendedTo],
		S.[Quantity] AS [IntendedQuantity],
		S.[NumberOfShipments] AS [NumberOfShipments],
		U.[Description] AS [Units],
		NULL AS [ConsentFrom],	-- TODO
		NULL AS [ConsentTo],	-- TODO
		'Import' AS [ImportOrExport]

	FROM [ImportNotification].[Notification] AS N

	INNER JOIN	[Lookup].[NotificationType] AS NT
	ON			[NT].[Id] = [N].[NotificationType]

	INNER JOIN	[Lookup].[UnitedKingdomCompetentAuthority] AS [CA]
	ON			[CA].[Id] = [N].[CompetentAuthority]

	LEFT JOIN	[ImportNotification].[Shipment] AS S
	ON			[N].[Id] = [S].[ImportNotificationId]

	LEFT JOIN	[Lookup].[ShipmentQuantityUnit] AS U
	ON			[S].[Units] = [U].[Id]
GO