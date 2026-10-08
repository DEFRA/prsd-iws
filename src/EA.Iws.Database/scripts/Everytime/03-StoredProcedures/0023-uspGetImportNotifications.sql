IF OBJECT_ID('[ImportNotification].[uspGetImportNotifications]') IS NULL
	EXEC('CREATE PROCEDURE [ImportNotification].[uspGetImportNotifications] AS SET NOCOUNT ON;')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- ======================================================================================
-- Author:		Sreedhar B
-- Create date: 19/06/2026
-- Description:	To get the list of import notifications to display advanced search result
-- ======================================================================================
ALTER PROCEDURE [ImportNotification].[uspGetImportNotifications]
	@NotificationIds NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		N.Id,
		N.NotificationNumber,
		S.Description AS Status,
		E.Name AS Exporter,
		CASE
			WHEN WT.BaselOecdCodeNotListed = 1 THEN 'Not listed'
			ELSE WC.Code
		END AS BaselOecdCode,
		CASE
			WHEN S.Description IN ('Consented', 'Consent Withdrawn') THEN CAST(1 AS BIT)
			ELSE CAST(0 AS BIT)
		END AS ShowShipmentSummaryLink
	FROM ImportNotification.Notification N
	INNER JOIN ImportNotification.NotificationAssessment NA ON N.Id = NA.NotificationApplicationId
	INNER JOIN Lookup.ImportNotificationStatus S ON NA.Status = S.Id
	LEFT JOIN ImportNotification.Exporter E ON N.Id = E.ImportNotificationId
	LEFT JOIN ImportNotification.WasteType WT ON N.Id = WT.ImportNotificationId
	LEFT JOIN ImportNotification.WasteCode W ON WT.Id = W.WasteTypeId
	INNER JOIN Lookup.WasteCode WC ON W.WasteCodeId = WC.Id AND WC.CodeType IN (1, 2)
	WHERE N.Id IN (@NotificationIds);
END
GO