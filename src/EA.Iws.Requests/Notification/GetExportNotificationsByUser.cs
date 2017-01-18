namespace EA.Iws.Requests.Notification
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanGetNotificationsForApplicantHome)]
    public class GetExportNotificationsByUser : IRequest<UserNotifications>
    {
        public GetExportNotificationsByUser(int pageNumber, NotificationStatus? notificationStatus)
        {
            PageNumber = pageNumber;
            NotificationStatus = notificationStatus;
        }

        public int PageNumber { get; private set; }

        public NotificationStatus? NotificationStatus { get; private set; }
    }
}