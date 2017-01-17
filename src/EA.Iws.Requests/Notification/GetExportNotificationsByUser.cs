namespace EA.Iws.Requests.Notification
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanGetNotificationsForApplicantHome)]
    public class GetExportNotificationsByUser : IRequest<UserNotifications>
    {
        public GetExportNotificationsByUser(int pageNumber)
        {
            PageNumber = pageNumber;
        }

        public int PageNumber { get; private set; }
    }
}