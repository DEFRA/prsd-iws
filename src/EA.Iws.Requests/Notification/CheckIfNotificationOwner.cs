namespace EA.Iws.Requests.Notification
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class CheckIfNotificationOwner : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public CheckIfNotificationOwner(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
