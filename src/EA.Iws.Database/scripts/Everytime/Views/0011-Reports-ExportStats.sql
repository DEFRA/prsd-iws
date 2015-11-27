IF OBJECT_ID('[Reports].[ExportStats]') IS NULL
    EXEC('CREATE VIEW [Reports].[ExportStats] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[ExportStats]
AS

SELECT
	SUM(QuantityReceived) AS QuantityReceived,
	[Year],
	WasteType,
	CountryOfImport,
	TransitStates,
	[BaselOecd],
	[BaselOecdDescription],
	[EWC],
	[Ycode],
	[Hcode],
	[HcodeDescription],
	[UN],
	OperationCodes
FROM (
	SELECT
		M.QuantityReceived,
		YEAR(M.ReceivedDate) AS Year,
		WT.Description AS WasteType,
		TR.ImportCountryCode AS [CountryOfImport],
		TS.TransitStates,
		(SELECT TOP 1 WC.Code
			FROM [Reports].[WasteCodes] WC
			WHERE WC.CodeType IN (1, 2) AND WC.NotificationId = M.NotificationId) AS [BaselOecd],
		(SELECT TOP 1 WC.Description
			FROM [Reports].[WasteCodes] WC
			WHERE WC.CodeType IN (1, 2) AND WC.NotificationId = M.NotificationId) AS [BaselOecdDescription],
		STUFF(( SELECT ', ' + WC.Code AS [text()]
				   FROM [Reports].[WasteCodes] WC
				   WHERE WC.NotificationId = M.NotificationId AND WC.CodeType = 3
				   order by 1
				   FOR XML PATH('')
				 ), 1, 1, '' ) AS [EWC],
		STUFF(( SELECT ', ' + WC.Code AS [text()]
				   FROM [Reports].[WasteCodes] WC
				   WHERE WC.NotificationId = M.NotificationId AND WC.CodeType = 4
				   order by 1
				   FOR XML PATH('')
				 ), 1, 1, '' ) AS [Ycode],
		STUFF(( SELECT ', ' + WC.Code AS [text()]
				   FROM [Reports].[WasteCodes] WC
				   WHERE WC.NotificationId = M.NotificationId AND WC.CodeType = 5
				   order by 1
				   FOR XML PATH('')
				 ), 1, 1, '' ) AS [Hcode],
		STUFF(( SELECT ', ' + WC.Description AS [text()]
				   FROM [Reports].[WasteCodes] WC
				   WHERE WC.NotificationId = M.NotificationId AND WC.CodeType = 5
				   order by 1
				   FOR XML PATH('')
				 ), 1, 1, '' ) AS [HcodeDescription],
		STUFF(( SELECT ', ' + WC.Code AS [text()]
				   FROM [Reports].[WasteCodes] WC
				   WHERE WC.NotificationId = M.NotificationId AND WC.CodeType = 6
				   order by 1
				   FOR XML PATH('')
				 ), 1, 1, '' ) AS [UN],
		OCC.OperationCodes
	FROM
		[Reports].[Movements] M
		INNER JOIN [Reports].[TransportRoute] TR ON TR.NotificationId = M.NotificationId
		LEFT JOIN [Reports].[TransitStatesConcat] TS ON TS.NotificationId = M.NotificationId
		INNER JOIN [Reports].[OperationCodesConcat] OCC ON OCC.NotificationId = M.NotificationId
		INNER JOIN [Reports].[WasteType] WT ON WT.NotificationId = M.NotificationId
	WHERE
		M.Status IN (3, 4)
) DATA
GROUP BY
	[Year],
	WasteType,
	CountryOfImport,
	TransitStates,
	[BaselOecd],
	[BaselOecdDescription],
	[EWC],
	[Ycode],
	[Hcode],
	[HcodeDescription],
	[UN],
	OperationCodes

GO