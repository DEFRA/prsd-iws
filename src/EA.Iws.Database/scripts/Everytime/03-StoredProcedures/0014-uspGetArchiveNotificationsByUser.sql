IF OBJECT_ID('[Notification].[uspGetArchiveNotificationsByUser]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspGetArchiveNotificationsByUser] AS SET NOCOUNT ON;')
GO
    ALTER PROCEDURE [Notification].[uspGetArchiveNotificationsByUser]
        @UserId UNIQUEIDENTIFIER,
        @Skip int,
        @Take int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        N.Id,
        N.NotificationNumber,
        LNS.[Description] AS [Status],
        CONVERT(varchar, N.CreatedDate, 23) AS [CreatedDate],
        E.[Name] AS CompanyName,
	    N.CompetentAuthority
    FROM
        [Notification].[Notification] N
        INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
        INNER JOIN [Lookup].NotificationStatus LNS on LNS.Id = NA.[Status]
        LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
    WHERE N.IsArchived = 0 AND N.CreatedDate < dateadd(year, -3, getdate())
	    AND NA.[Status] IN (14,8,9,11)
	    AND N.CompetentAuthority IN (select CompetentAuthority from [Person].[InternalUser] where UserId = @UserID)
    UNION 
    SELECT 
	    INN.Id,
	    INN.NotificationNumber,
	    LINS.[Description] as [Status],
        CONVERT(varchar, INND.NotificationReceivedDate, 23) AS [CreatedDate],
	    INE.[Name] as CompanyName,
	    INN.CompetentAuthority
    FROM [ImportNotification].[Notification] INN
        INNER JOIN [ImportNotification].[NotificationAssessment] INNA ON INN.Id = INNA .NotificationApplicationId
	    INNER JOIN [ImportNotification].[NotificationDates] INND ON INND.NotificationAssessmentId = INNA.Id
		INNER JOIN [Lookup].ImportNotificationStatus LINS ON LINS.Id = INNA.[Status]
	    LEFT JOIN [ImportNotification].[Exporter] INE on INN.Id = INE.ImportNotificationId
    WHERE INN.IsArchived = 0 AND INND.NotificationReceivedDate < dateadd(year, -3, getdate())
	    AND INNA.[Status] IN (13,12,11,10)
	    AND INN.CompetentAuthority IN (select CompetentAuthority from [Person].[InternalUser] where UserId = @UserID)

    ORDER by CreatedDate Asc
    OFFSET (@Skip) ROWS FETCH NEXT (@Take) ROWS ONLY

END
GO