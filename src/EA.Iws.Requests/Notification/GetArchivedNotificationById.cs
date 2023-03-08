namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetArchivedNotificationById : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public GetArchivedNotificationById(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
