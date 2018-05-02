IF OBJECT_ID('[Reports].[DataImportNotifications]') IS NULL
    EXEC('CREATE VIEW [Reports].[DataImportNotifications] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[DataImportNotifications]
AS
    SELECT 
        REPLACE(N.[NotificationNumber], ' ', '') AS [NotificationNumber],
        N.[NotificationType],
        N.[CompetentAuthority] AS [CompetentAuthorityId],
        FC.[AllFacilitiesPreconsented] AS [Preconsented],
        NA.[Status],
        D.[NotificationReceivedDate] AS [NotificationReceived],
        D.[PaymentReceivedDate] AS [PaymentReceived],
        D.[AssessmentStartedDate] AS [AssessmentStarted],
        D.[NotificationCompletedDate] AS [ApplicationCompleted],		
        D.[AcknowledgedDate] AS [Acknowledged],
        C.[From] AS [Consented],
        D.[NameOfOfficer] AS [Officer],
        -- Decision date will be the date it was withdrawn, objected or consented and it will only be one of these.
        CAST(COALESCE(D.WithdrawnDate, COALESCE(O.[Date], D.[ConsentedDate])) AS DATE) AS [DecisionDate]        

    FROM [ImportNotification].[Notification] AS [N]

    LEFT JOIN   [ImportNotification].[FacilityCollection] AS FC
    ON			[N].[Id] = [FC].[ImportNotificationId]

    INNER JOIN  [ImportNotification].[NotificationAssessment] AS NA
    ON			[N].[Id] = [NA].[NotificationApplicationId]

    INNER JOIN	[ImportNotification].[NotificationDates] AS D
    ON			[D].[NotificationAssessmentId] = [NA].[Id]

    LEFT JOIN	[ImportNotification].[Consent] AS C
    ON			[N].[Id] = [C].[NotificationId]

    LEFT JOIN	[ImportNotification].[Objection] AS O
    ON			[N].[Id] = [O].[NotificationId]

    WHERE		[NA].[Status] NOT IN (1)
GO