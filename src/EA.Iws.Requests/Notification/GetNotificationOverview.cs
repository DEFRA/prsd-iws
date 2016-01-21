namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification.Overview;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetNotificationOverview : IRequest<NotificationOverview>
    {
        public Guid NotificationId { get; set; }

        public GetNotificationOverview(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
