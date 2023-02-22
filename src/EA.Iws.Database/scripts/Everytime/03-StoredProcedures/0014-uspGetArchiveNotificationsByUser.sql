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
    SELECT
        N.Id,
        N.NotificationNumber,
        LNS.[Description] AS [Status],
		CASE 
			WHEN Na.[Status] = 14 THEN CONVERT(VARCHAR, ND.FileClosedDate, 103)
			WHEN Na.[Status] = 8 THEN CONVERT(VARCHAR, ND.WithdrawnDate, 103)
			WHEN Na.[Status] = 9 THEN CONVERT(VARCHAR, ND.ObjectedDate, 103)
			WHEN Na.[Status] = 11 THEN CONVERT(VARCHAR, ND.ConsentWithdrawnDate, 103)
			ELSE CONVERT(VARCHAR, N.CreatedDate, 103) 
		END AS [DateActioned],
        N.CreatedDate AS OrderByDate,
        E.[Name] AS CompanyName,
	    N.CompetentAuthority
    FROM
        [Notification].[Notification] N
        INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
        INNER JOIN [Lookup].NotificationStatus LNS ON LNS.Id = NA.[Status]
		LEFT JOIN [Notification].[NotificationDates] ND on ND.NotificationAssessmentId = NA.Id
        LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
    WHERE N.IsArchived = 0 AND N.CreatedDate < dateadd(year, -3, getdate())
	    AND NA.[Status] IN (14,8,9,11)
	    AND N.CompetentAuthority IN (SELECT CompetentAuthority FROM [Person].[InternalUser] WHERE UserId = @UserID)
    UNION 
    SELECT 
	    INN.Id,
	    INN.NotificationNumber,
	    LINS.[Description] AS [Status],
		CASE 
			WHEN INNA.[Status] = 10 THEN CONVERT(VARCHAR, INND.ConsentWithdrawnDate, 103)
			WHEN INNA.[Status] = 12 THEN CONVERT(VARCHAR, INND.WithdrawnDate, 103)
			WHEN INNA.[Status] = 13 THEN CONVERT(VARCHAR, INND.FileClosedDate, 103)
			--We don't have a column in the NotificationDates that matches the Status 11, 
			--and this doesn't seem to be a valid state for importnotifications, so ignoring
			ELSE CONVERT(VARCHAR, INND.NotificationReceivedDate, 103) 
		END AS [DateActioned],
        INND.NotificationReceivedDate AS OrderByDate,
	    INE.[Name] AS CompanyName,
	    INN.CompetentAuthority
    FROM [ImportNotification].[Notification] INN
        INNER JOIN [ImportNotification].[NotificationAssessment] INNA ON INN.Id = INNA .NotificationApplicationId
	    INNER JOIN [ImportNotification].[NotificationDates] INND ON INND.NotificationAssessmentId = INNA.Id
		INNER JOIN [Lookup].ImportNotificationStatus LINS ON LINS.Id = INNA.[Status]
	    LEFT JOIN [ImportNotification].[Exporter] INE ON INN.Id = INE.ImportNotificationId
    WHERE INN.IsArchived = 0 AND INND.NotificationReceivedDate < dateadd(year, -3, getdate())
	    AND INNA.[Status] IN (13,12,11,10)
	    AND INN.CompetentAuthority IN (SELECT CompetentAuthority FROM [Person].[InternalUser] WHERE UserId = @UserID)

    ORDER by [OrderByDate] ASC
    OFFSET (@Skip) ROWS FETCH NEXT (@Take) ROWS ONLY

END
GO