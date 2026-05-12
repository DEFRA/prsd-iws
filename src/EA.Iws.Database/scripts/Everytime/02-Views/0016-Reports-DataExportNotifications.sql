IF OBJECT_ID('[Reports].[DataExportNotifications]') IS NULL
    EXEC('CREATE VIEW [Reports].[DataExportNotifications] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[DataExportNotifications]
AS
	SELECT
		REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
		N.NotificationType,
		N.CompetentAuthority AS CompetentAuthorityId,
		ISNULL(FC.AllFacilitiesPreconsented, 'false') AS Preconsented,
		NA.[Status],
		D.NotificationReceivedDate	AS NotificationReceived,
		D.PaymentReceivedDate		AS PaymentReceived,
		D.CommencementDate			AS AssessmentStarted,
		D.CompleteDate				AS ApplicationCompleted,
		D.TransmittedDate			AS Transmitted,
		D.AcknowledgedDate			AS Acknowledged,
		C.[From] AS Consented,
		C.[To]   AS ConsentTo,
		D.NameOfOfficer AS Officer,
		-- Only one of these dates should exist
		CAST(
			COALESCE(
				D.WithdrawnDate,
				D.ObjectedDate,
				D.ConsentedDate
			) AS DATE
		) AS DecisionDate,
		SB.SubmittedBy,
		CAST(SB.SubmittedDate AS DATE) AS SubmittedDate,
		CAST(D.ConsentWithdrawnDate AS DATE) AS ConsentWithdrawnDate
	FROM [Notification].[Notification] N
	LEFT JOIN [Notification].[FacilityCollection] FC ON FC.NotificationId = N.Id
	INNER JOIN [Notification].[NotificationAssessment] NA ON NA.NotificationApplicationId = N.Id
	INNER JOIN [Notification].[NotificationDates] D ON D.NotificationAssessmentId = NA.Id
	LEFT JOIN [Notification].[Consent] C ON C.NotificationApplicationId = N.Id
	OUTER APPLY
	(
		SELECT TOP (1)
			CASE
				WHEN IU.Id IS NULL THEN 'External User'
				ELSE 'Internal User'
			END AS SubmittedBy,
		CONVERT(varchar(10), NSC.ChangeDate, 23) AS SubmittedDate
		FROM [Notification].[NotificationStatusChange] NSC
		LEFT JOIN [Person].[InternalUser] IU ON IU.UserId = NSC.UserId
		WHERE
			NSC.NotificationAssessmentId = NA.Id
			AND NSC.[Status] = 2 -- Submitted
		ORDER BY NSC.ChangeDate ASC
	) SB

	WHERE NA.[Status] <> 1;
GO