IF OBJECT_ID('[Notification].[uspGetArchiveNotificationsByUser]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspGetArchiveNotificationsByUser] AS SET NOCOUNT ON;')
GO
    ALTER PROCEDURE [Notification].[uspGetArchiveNotificationsByUser]
        @UserId UNIQUEIDENTIFIER,
        @Skip INT,
        @Take INT
AS
BEGIN
    SET NOCOUNT ON;
	select [Id], [NotificationNumber], [Status], [DateActioned], [DateActioned] as [OrderByDate], [CompanyName], [CompetentAuthority] from (
    SELECT
        N.Id,
        N.NotificationNumber,
        LNS.[Description] AS [Status],
		CASE 
			WHEN Na.[Status] = 8 THEN CONVERT(VARCHAR, ND.WithdrawnDate, 103)
			WHEN Na.[Status] = 9 THEN CONVERT(VARCHAR, ND.ObjectedDate, 103)
			WHEN Na.[Status] = 11 THEN CONVERT(VARCHAR, ND.ConsentWithdrawnDate, 103)
			WHEN Na.[Status] = 14 THEN CONVERT(VARCHAR, ND.FileClosedDate, 103)
		END AS [DateActioned],
        E.[Name] AS CompanyName,
	    N.CompetentAuthority
    FROM
        [Notification].[Notification] N
        INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
        INNER JOIN [Lookup].NotificationStatus LNS ON LNS.Id = NA.[Status]
		LEFT JOIN [Notification].[NotificationDates] ND on ND.NotificationAssessmentId = NA.Id
        LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
    WHERE N.IsArchived = 0 
		AND CASE 
			WHEN Na.[Status] = 8 THEN ND.WithdrawnDate
			WHEN Na.[Status] = 9 THEN ND.ObjectedDate
			WHEN Na.[Status] = 11 THEN ND.ConsentWithdrawnDate
			WHEN Na.[Status] = 14 THEN ND.FileClosedDate
		END < dateadd(year, -3, getdate())
	    AND NA.[Status] IN (8,9,11,14)
	    AND N.CompetentAuthority IN (SELECT CompetentAuthority FROM [Person].[InternalUser] WHERE UserId = @UserID)
    UNION 
    SELECT 
	    INN.Id,
	    INN.NotificationNumber,
	    LINS.[Description] AS [Status],
		CASE 
			WHEN INNA.[Status] = 10 THEN CONVERT(VARCHAR, INND.ConsentWithdrawnDate, 103)
			WHEN INNA.[Status] = 11 THEN CONVERT(VARCHAR, W.[Date], 103)
			WHEN INNA.[Status] = 12 THEN CONVERT(VARCHAR, O.[Date], 103)
			WHEN INNA.[Status] = 13 THEN CONVERT(VARCHAR, INND.FileClosedDate, 103)
		END AS [DateActioned],
	    INE.[Name] AS CompanyName,
	    INN.CompetentAuthority
    FROM [ImportNotification].[Notification] INN
        INNER JOIN [ImportNotification].[NotificationAssessment] INNA ON INN.Id = INNA .NotificationApplicationId
	    INNER JOIN [ImportNotification].[NotificationDates] INND ON INND.NotificationAssessmentId = INNA.Id
		INNER JOIN [Lookup].ImportNotificationStatus LINS ON LINS.Id = INNA.[Status]
	    LEFT JOIN [ImportNotification].[Exporter] INE ON INN.Id = INE.ImportNotificationId
		LEFT JOIN [ImportNotification].[Withdrawn] W ON W.NotificationId = INN.Id
		LEFT JOIN [ImportNotification].[Objection] O ON O.NotificationId = INN.Id
    WHERE INN.IsArchived = 0 
		AND CASE 
			WHEN INNA.[Status] = 10 THEN INND.ConsentWithdrawnDate
			WHEN INNA.[Status] = 11 THEN W.[Date]
			WHEN INNA.[Status] = 12 THEN O.[Date]
			WHEN INNA.[Status] = 13 THEN INND.FileClosedDate
		END < dateadd(year, -3, getdate())
	    AND INNA.[Status] IN (10,11,12,13)
	    AND INN.CompetentAuthority IN (SELECT CompetentAuthority FROM [Person].[InternalUser] WHERE UserId = @UserID)
	) archiveNotifications	
    ORDER by [OrderByDate] ASC
    OFFSET (@Skip) ROWS FETCH NEXT (@Take) ROWS ONLY
END
GO