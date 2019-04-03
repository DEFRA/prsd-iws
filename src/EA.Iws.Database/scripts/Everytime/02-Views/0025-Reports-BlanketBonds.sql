﻿IF OBJECT_ID('[Reports].[BlanketBonds]') IS NULL
    EXEC('CREATE VIEW [Reports].[BlanketBonds] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[BlanketBonds]
AS

SELECT
	[ReferenceNumber],
	[ApprovedDate],
	[ActiveLoadsPermitted],
	[CurrentActiveLoads],
	[NotificationNumber],
	[ExporterName],
	[ImporterName],
	[ProducerName],
	[CompetentAuthority],
	[IsBlanketBond],
	[CoverAmount],
	[CalculationContinued],
	CASE
		WHEN [CurrentActiveLoads] > [ActiveLoadsPermitted] THEN 'Y'
		WHEN [CurrentActiveLoads] <= [ActiveLoadsPermitted] THEN 'N'
		ELSE NULL 
	END AS [OverActiveLoads]
FROM
(
	SELECT 
		FG.[ReferenceNumber],
		FG.[DecisionDate] AS [ApprovedDate],
		FG.[ActiveLoadsPermitted],
		(SELECT COUNT(Id) FROM [Notification].[Movement] M
			WHERE M.[NotificationId] = N.Id 
			AND M.[Status] IN (2, 3)
			AND M.[Date] <= GETDATE()) AS [CurrentActiveLoads],
		REPLACE(N.[NotificationNumber], ' ', '') AS [NotificationNumber],
		E.[Name] AS [ExporterName],
		I.[Name] AS [ImporterName],
		P.[Name] AS [ProducerName],
		N.[CompetentAuthority],
		CASE
			WHEN FG.[IsBlanketBond] = 1 THEN 'Yes'
			WHEN FG.[IsBlanketBond] = 0 THEN 'No'
			ELSE '- -' 
		END AS [IsBlanketBond],
		FG.[CoverAmount],
		FG.[CalculationContinued]
	FROM
		[Notification].[Notification] N
		INNER JOIN  [Notification].[FinancialGuaranteeCollection] FGC
		ON N.[Id] = FGC.[NotificationId]

		INNER JOIN [Notification].[FinancialGuarantee] FG
		ON FG.[FinancialGuaranteeCollectionId] = FGC.[Id]	

		INNER JOIN [Notification].[Exporter] E
		ON N.[Id] = E.[NotificationId]

		INNER JOIN [Notification].[Importer] I
		ON N.[Id] = I.[NotificationId]

		INNER JOIN [Notification].[ProducerCollection] PC
			INNER JOIN [Notification].[Producer] P
			ON PC.[Id] = P.[ProducerCollectionId]
			AND P.[IsSiteOfExport] = 1
		ON N.[Id] = PC.[NotificationId]
    
		WHERE FG.[Decision] = 3 -- Approved
		AND FG.[Status] = 4 -- Approved
) subquery

GO