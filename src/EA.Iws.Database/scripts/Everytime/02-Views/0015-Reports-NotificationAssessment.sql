IF OBJECT_ID('[Reports].[NotificationAssessment]') IS NULL
    EXEC('CREATE VIEW [Reports].[NotificationAssessment] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[NotificationAssessment]
AS
    SELECT
        NA.[Id] AS Id,
        NA.[NotificationApplicationId] AS NotificationId,
        NA.[Status] AS [ExportStatusId],
        NULL AS [ImportStatusId],
        S.[Description] AS [Status],
        CAST(D.[PaymentReceivedDate] AS DATE) AS [PaymentReceivedDate],
        CAST(D.[AcknowledgedDate] AS DATE) AS [AcknowlegedDate],
        CAST(D.[CommencementDate] AS DATE) AS [CommencementDate],
        D.NameOfOfficer AS [Officer],
        CAST(D.[CompleteDate] AS DATE) AS [CompleteDate],
        CAST(D.[NotificationReceivedDate] AS DATE) AS [ReceivedDate],
        CAST(D.[TransmittedDate] AS DATE) AS [TransmittedDate],
        CAST(D.[WithdrawnDate] AS DATE) AS [WithdrawnDate],
        D.[WithdrawnReason],
        CAST(D.[ObjectedDate] AS DATE) AS [ObjectedDate],
        D.[ObjectionReason],
        CAST(C.[From] AS DATE) AS [ConsentFrom],
        CAST(C.[To] AS DATE) AS [ConsentTo],
        CAST(D.[ConsentedDate] AS DATE) AS [ConsentedDate],
        C.[Conditions] AS [ConsentConditions],
        CASE
            WHEN C.[Id] IS NOT NULL AND S.[Description] = 'Consented' THEN 1
            ELSE 0
        END AS [IsConsented],
        CASE
            WHEN C.[To] < GETUTCDATE() THEN 1
            ELSE 0
        END AS [IsConsentExpired],
        'Export' AS [ImportOrExport]

    FROM		[Notification].[NotificationAssessment] AS NA

    INNER JOIN	[Lookup].[NotificationStatus] AS S
    ON			[S].[Id] = [NA].[Status]

    INNER JOIN	[Notification].[NotificationDates] AS D
    ON			[D].[NotificationAssessmentId] = [NA].[Id]

    LEFT JOIN	[Notification].[Consent] AS C
    ON			[NA].[NotificationApplicationId] = [C].[NotificationApplicationId]

    UNION ALL

        SELECT
        NA.[Id] AS Id,
        NA.[NotificationApplicationId] AS NotificationId,
        NULL AS [ExportStatusId],
        NA.[Status] AS [ImportStatusId],
        S.[Description] AS [Status],
        CAST(D.[PaymentReceivedDate] AS DATE) AS [PaymentReceivedDate],
        CAST(D.[AcknowledgedDate] AS DATE) AS [AcknowlegedDate],
        CAST(D.[AssessmentStartedDate] AS DATE) AS [CommencementDate],
        D.NameOfOfficer AS [Officer],
        CAST(D.[NotificationCompletedDate] AS DATE) AS [CompleteDate],
        CAST(D.[NotificationReceivedDate] AS DATE) AS [ReceivedDate],
        NULL AS [TransmittedDate],
        CAST(W.[Date] AS DATE) AS [WithdrawnDate],
        W.[Reasons] AS [WithdrawnReason],
        CAST(O.[Date] AS DATE) AS [ObjectedDate],
        O.[Reasons] AS [ObjectionReason],
        CAST(C.[From] AS DATE) AS [ConsentFrom],
        CAST(C.[To] AS DATE) AS [ConsentTo],
        CAST(D.[ConsentedDate] AS DATE) AS [ConsentedDate],
        C.[Conditions] AS [ConsentConditions],
        CASE
            WHEN C.[Id] IS NOT NULL AND S.[Description] = 'Consented' THEN 1
            ELSE 0
        END AS [IsConsented],
        CASE
            WHEN C.[To] < GETUTCDATE() THEN 1
            ELSE 0
        END AS [IsConsentExpired],
        'Import' AS [ImportOrExport]

    FROM		[ImportNotification].[NotificationAssessment] AS NA

    INNER JOIN	[Lookup].[ImportNotificationStatus] AS S
    ON			[S].[Id] = [NA].[Status]

    INNER JOIN	[ImportNotification].[NotificationDates] AS D
    ON			[D].[NotificationAssessmentId] = [NA].[Id]

    LEFT JOIN	[ImportNotification].[Consent] AS C
    ON			[NA].[NotificationApplicationId] = [C].[NotificationId]

    LEFT JOIN	[ImportNotification].[Objection] AS O
    ON			[NA].[NotificationApplicationId] = [O].[NotificationId]

    LEFT JOIN	[ImportNotification].[Withdrawn] AS W
    ON			[NA].[NotificationApplicationId] = [W].[NotificationId]
GO