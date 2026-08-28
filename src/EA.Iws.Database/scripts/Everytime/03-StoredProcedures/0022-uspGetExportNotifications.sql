IF OBJECT_ID('[Notification].[uspGetExportNotifications]') IS NULL
	EXEC('CREATE PROCEDURE [Notification].[uspGetExportNotifications] AS SET NOCOUNT ON;')
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =====================================================================================
-- Author:		Sreedhar B
-- Create date: 19/06/2026
-- Description:	To get the list of export notifications to display advanced search result
-- ======================================================================================
ALTER PROCEDURE [Notification].[uspGetExportNotifications]
(
	@NotificationIds NVARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		N.Id,
		N.NotificationNumber,
		NS.Description AS NotificationStatus,
		E.Name AS ExporterName,
		CCT.Description AS WasteType,
		CASE
			WHEN NS.Description IN ('Consented', 'Consent Withdrawn')
			AND FG.Id IS NOT NULL
			THEN CAST(1 AS BIT)
			ELSE CAST(0 AS BIT)
		END AS ShowShipmentSummaryLink
	FROM [Notification].[Notification] N
	INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
	INNER JOIN [Lookup].[NotificationStatus] NS ON NA.Status = NS.Id
	LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
	LEFT JOIN [Notification].[WasteType] WT ON N.Id = WT.NotificationId
	LEFT JOIN [Lookup].[ChemicalCompositionType] CCT ON WT.ChemicalCompositionType = CCT.Id
	LEFT JOIN [Notification].[FinancialGuaranteeCollection] FGC ON FGC.NotificationId = N.Id
	OUTER APPLY
	(
		SELECT TOP (1)
			FG1.Id
		FROM [Notification].[FinancialGuarantee] FG1
		WHERE FG1.FinancialGuaranteeCollectionId = FGC.Id
		AND FG1.Status IN (4, 6)
	) FG
WHERE N.Id IN
(
    SELECT CAST(x.i.value('.', 'nvarchar(50)') AS UNIQUEIDENTIFIER)
    FROM
    (
        SELECT CAST(
            '<i>' + REPLACE(@NotificationIds, ',', '</i><i>') + '</i>'
            AS XML
        ) AS XmlData
    ) d
    CROSS APPLY XmlData.nodes('/i') x(i)
)
END;
GO
