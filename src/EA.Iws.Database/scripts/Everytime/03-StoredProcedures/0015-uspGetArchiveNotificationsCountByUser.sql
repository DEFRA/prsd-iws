IF OBJECT_ID('[Notification].[uspGetArchiveNotificationsCountByUser]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspGetArchiveNotificationsCountByUser] AS SET NOCOUNT ON;')
GO
    ALTER PROCEDURE [Notification].[uspGetArchiveNotificationsByUser]
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
        LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
    WHERE n.CreatedDate < dateadd(year, -3, getdate())
	    AND NA.[Status] IN (14,8,9,11)
	    AND N.CompetentAuthority IN (select CompetentAuthority from [Person].[InternalUser] where UserId = @UserID)
    UNION 
    SELECT 
	    INN.id
    FROM [ImportNotification].[Notification] INN
        INNER JOIN [ImportNotification].[NotificationAssessment] INNA ON INN.Id = INNA .NotificationApplicationId
	    INNER JOIN [ImportNotification].[NotificationDates] INND ON INND.NotificationAssessmentId = INNA.Id
	    LEFT JOIN [ImportNotification].[Exporter] INE on INN.Id = INE.ImportNotificationId
    WHERE INND.NotificationReceivedDate < dateadd(year, -3, getdate())
	    AND INNA.[Status] IN (13,12,11,10)
	    AND INN.CompetentAuthority IN (select CompetentAuthority from [Person].[InternalUser] where UserId = @UserID)
    ) ArchiveNotifications
END
GO