IF OBJECT_ID('[Reports].[WasteType]') IS NULL
    EXEC('CREATE VIEW [Reports].[WasteType] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[WasteType]
AS

	SELECT
		N.Id AS NotificationId,
		N.NotificationNumber,
		CASE 
			WHEN WT.ChemicalCompositionType IN (1, 2) THEN CCT.Description
			WHEN WT.ChemicalCompositionType = 3 THEN WT.WoodTypeDescription
			WHEN WT.ChemicalCompositionType = 4 THEN WT.ChemicalCompositionName
		END AS Description
	FROM
		[Notification].[Notification] N
		INNER JOIN [Notification].[WasteType] WT ON WT.NotificationId = N.Id
		INNER JOIN [Lookup].[ChemicalCompositionType] CCT ON WT.ChemicalCompositionType = CCT.Id
GO