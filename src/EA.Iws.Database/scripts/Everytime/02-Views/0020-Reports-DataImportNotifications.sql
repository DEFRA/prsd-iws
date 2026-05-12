IF OBJECT_ID('[Reports].[DataImportNotifications]') IS NULL
    EXEC('CREATE VIEW [Reports].[DataImportNotifications] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[DataImportNotifications]
AS
	SELECT
		REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
		N.NotificationType,
		N.CompetentAuthority AS CompetentAuthorityId,
		ISNULL(FC.AllFacilitiesPreconsented, 'false') AS Preconsented,
		NA.[Status],
		D.NotificationReceivedDate   AS NotificationReceived,
		D.PaymentReceivedDate        AS PaymentReceived,
		D.AssessmentStartedDate      AS AssessmentStarted,
		D.NotificationCompletedDate  AS ApplicationCompleted,
		D.AcknowledgedDate           AS Acknowledged,
		C.[From] AS Consented,
		C.[To]   AS ConsentTo,
		D.NameOfOfficer AS Officer,
		-- Only one decision date should exist
		CAST
		(
			COALESCE
			(
				D.WithdrawnDate,
				O.[Date],
				D.ConsentedDate
			) AS DATE
		) AS DecisionDate,
		CAST(SB.SubmittedDate AS DATE) AS SubmittedDate,
		CAST(D.ConsentWithdrawnDate AS DATE) AS ConsentWithdrawnDate
	FROM [ImportNotification].[Notification] N
	LEFT JOIN [ImportNotification].[FacilityCollection] FC ON FC.ImportNotificationId = N.Id
	INNER JOIN [ImportNotification].[NotificationAssessment] NA ON NA.NotificationApplicationId = N.Id
	INNER JOIN [ImportNotification].[NotificationDates] D ON D.NotificationAssessmentId = NA.Id
	LEFT JOIN [ImportNotification].[Consent] C ON C.NotificationId = N.Id
	LEFT JOIN [ImportNotification].[Objection] O ON O.NotificationId = N.Id
	OUTER APPLY
	(
		SELECT TOP (1)
			CONVERT(varchar(10), NSC.ChangeDate, 23) AS SubmittedDate
		FROM [ImportNotification].[NotificationStatusChange] NSC
		WHERE
			NSC.NotificationAssessmentId = NA.Id
			AND NSC.NewStatus = 2 -- Submitted
		ORDER BY NSC.ChangeDate ASC
	) SB

	WHERE NA.[Status] <> 1;
GO