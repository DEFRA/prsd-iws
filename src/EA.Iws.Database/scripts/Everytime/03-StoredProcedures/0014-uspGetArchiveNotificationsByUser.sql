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
        CONVERT(VARCHAR, N.CreatedDate, 103) AS [CreatedDate],
        N.CreatedDate AS OrderByDate,
        E.[Name] AS CompanyName,
	    N.CompetentAuthority
    FROM
        [Notification].[Notification] N
        INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
        INNER JOIN [Lookup].NotificationStatus LNS ON LNS.Id = NA.[Status]
        LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
    WHERE N.IsArchived = 0 AND N.CreatedDate < dateadd(year, -3, getdate())
	    AND NA.[Status] IN (14,8,9,11)
	    AND N.CompetentAuthority IN (SELECT CompetentAuthority FROM [Person].[InternalUser] WHERE UserId = @UserID)
    UNION 
    SELECT 
	    INN.Id,
	    INN.NotificationNumber,
	    LINS.[Description] AS [Status],
        CONVERT(VARCHAR, INND.NotificationReceivedDate, 103) AS [CreatedDate],
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