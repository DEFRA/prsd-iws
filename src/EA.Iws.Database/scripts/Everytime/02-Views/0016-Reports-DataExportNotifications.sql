IF OBJECT_ID('[Reports].[DataExportNotifications]') IS NULL
    EXEC('CREATE VIEW [Reports].[DataExportNotifications] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[DataExportNotifications]
AS
    SELECT 
        REPLACE(N.[NotificationNumber], ' ', '') AS [NotificationNumber],
        N.[NotificationType],
        N.[CompetentAuthority] AS [CompetentAuthorityId],
        FC.[AllFacilitiesPreconsented] AS [Preconsented],
        NA.[Status],
        D.[NotificationReceivedDate] AS [NotificationReceived],
        D.[PaymentReceivedDate] AS [PaymentReceived],
        D.[CommencementDate] AS [AssessmentStarted],
        D.[CompleteDate] AS [ApplicationCompleted],		
        D.[TransmittedDate] AS [Transmitted],
        D.[AcknowledgedDate] AS [Acknowledged],
        C.[From] AS [Consented],
        D.[NameOfOfficer] AS [Officer],
        -- Decision date will be the date it was withdrawn, objected or consented and it will only be one of these.
        CAST(COALESCE(D.WithdrawnDate, COALESCE(D.[ObjectedDate], D.[ConsentedDate])) AS DATE) AS [DecisionDate],
        [SubmittedBy].[SubmittedBy],
		C.[To] AS [ConsentTo]
    FROM [Notification].[Notification] AS [N]

    LEFT JOIN   [Notification].[FacilityCollection] AS FC
    ON			[N].[Id] = [FC].[NotificationId]

    INNER JOIN  [Notification].[NotificationAssessment] AS NA
    ON			[N].[Id] = [NA].[NotificationApplicationId]

    INNER JOIN	[Notification].[NotificationDates] AS D
    ON			[D].[NotificationAssessmentId] = [NA].[Id]

    LEFT JOIN	[Notification].[Consent] AS C
    ON			[N].[Id] = [C].[NotificationApplicationId]

    OUTER APPLY (SELECT TOP 1 CASE WHEN [IU].[Id] IS NULL THEN 'External User' ELSE 'Internal User' END AS [SubmittedBy]
                 FROM [Notification].[NotificationStatusChange] [NSC]
                 LEFT JOIN [Person].[InternalUser] IU ON [NSC].[UserId] = [IU].[UserId]
                 WHERE [NSC].[NotificationAssessmentId] = [NA].[Id] 
                 AND [NSC].[Status] = 2 -- Submitted
                 ORDER BY [ChangeDate] ASC) [SubmittedBy]

    WHERE		[NA].[Status] <> 1
GO