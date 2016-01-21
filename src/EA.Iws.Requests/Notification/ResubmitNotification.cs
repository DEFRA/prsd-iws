namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanSubmitExportNotification)]
    public class ResubmitNotification : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public ResubmitNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}