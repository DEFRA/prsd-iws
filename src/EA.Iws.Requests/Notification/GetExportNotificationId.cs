namespace EA.Iws.Requests.Notification
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.Notification;
    using EA.Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetExportNotificationId : IRequest<DeleteExportNotificationDetails>
    {
        public string NotificationNumber { get; private set; }

        public GetExportNotificationId(string notificationNumber)
        {
            NotificationNumber = notificationNumber;
        }
    }
}
