namespace EA.Iws.Requests.Notification
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class CheckNotificationNumberUnique : IRequest<bool>
    {
        public CheckNotificationNumberUnique(int notificationNumber, CompetentAuthority competentAuthority)
        {
            NotificationNumber = notificationNumber;
            CompetentAuthority = competentAuthority;
        }

        public int NotificationNumber { get; private set; }

        public CompetentAuthority CompetentAuthority { get; private set; }
    }
}