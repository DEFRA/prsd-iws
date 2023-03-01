IF OBJECT_ID('[Notification].[uspGetArchiveNotificationsCountByUser]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspGetArchiveNotificationsCountByUser] AS SET NOCOUNT ON;')
GO
    ALTER PROCEDURE [Notification].[uspGetArchiveNotificationsCountByUser]
        @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    select count(id) as NotificationIdCount FROM(
    SELECT
        N.Id
    FROM
        [Notification].[Notification] N
        INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
		LEFT JOIN [Notification].[NotificationDates] ND on ND.NotificationAssessmentId = NA.Id
        LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
    WHERE N.IsArchived = 0 
		AND CASE 
			WHEN Na.[Status] = 8 THEN ND.WithdrawnDate
			WHEN Na.[Status] = 9 THEN ND.ObjectedDate
			WHEN Na.[Status] = 11 THEN ND.ConsentWithdrawnDate
			WHEN Na.[Status] = 14 THEN ND.FileClosedDate
		END < dateadd(year, -3, getdate())
	    AND NA.[Status] IN (14,8,9,11)
	    AND N.CompetentAuthority IN (select CompetentAuthority from [Person].[InternalUser] where UserId = @UserID)
    UNION 
    SELECT 
	    INN.id
    FROM [ImportNotification].[Notification] INN
        INNER JOIN [ImportNotification].[NotificationAssessment] INNA ON INN.Id = INNA .NotificationApplicationId
	    INNER JOIN [ImportNotification].[NotificationDates] INND ON INND.NotificationAssessmentId = INNA.Id
	    LEFT JOIN [ImportNotification].[Exporter] INE on INN.Id = INE.ImportNotificationId
		LEFT JOIN [ImportNotification].[Withdrawn] W ON W.NotificationId = INN.Id
		LEFT JOIN [ImportNotification].[Objection] O ON O.NotificationId = INN.Id
    WHERE INN.IsArchived = 0 
		AND CASE 
			WHEN INNA.[Status] = 10 THEN INND.ConsentWithdrawnDate
			WHEN INNA.[Status] = 11 THEN W.[Date]
			WHEN INNA.[Status] = 12 THEN O.[Date]
			WHEN INNA.[Status] = 13 THEN INND.FileClosedDate
		END < dateadd(year, -3, getdate())
	    AND INNA.[Status] IN (13,12,11,10)
	    AND INN.CompetentAuthority IN (select CompetentAuthority from [Person].[InternalUser] where UserId = @UserID)
    ) ArchiveNotifications
END
GO