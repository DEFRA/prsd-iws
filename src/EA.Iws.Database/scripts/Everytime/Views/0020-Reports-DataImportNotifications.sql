IF OBJECT_ID('[Reports].[DataImportNotifications]') IS NULL
    EXEC('CREATE VIEW [Reports].[DataImportNotifications] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[DataImportNotifications]
AS
    SELECT
        N.[NotificationNumber],
        N.[TypeId] AS [NotificationType],
        N.[CompetentAuthorityId],
        N.[Preconsented],
        NA.[ImportStatusId] AS [Status],
        NA.[ReceivedDate] AS [NotificationReceived],
        NA.[PaymentReceivedDate] AS [PaymentReceived],
        NA.[CommencementDate] AS [AssessmentStarted],
        NA.[CompleteDate] AS [ApplicationCompleted],
        NA.[AcknowlegedDate] AS [Acknowledged],
		C.[From] AS [Consented],
		NA.[Officer],
        -- Decision date will be the date it was withdrawn, objected or consented and it will only be one of these.
        CAST(COALESCE(NA.WithdrawnDate, COALESCE(NA.ObjectedDate, NA.ConsentedDate)) AS DATE) AS [DecisionDate]

    FROM		[Reports].[NotificationAssessment] AS NA

    INNER JOIN	[Reports].[Notification] AS N
    ON			[N].[Id] = [NA].[NotificationId]

	LEFT JOIN	[ImportNotification].[Consent] AS C
	ON			[NA].[NotificationId] = [C].[NotificationId]

    WHERE		[NA].[ImportOrExport] = 'Import'
    AND			[NA].[ImportStatusId] <> 1
GO