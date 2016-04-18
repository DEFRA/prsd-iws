namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification.Overview;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationInternal)]
    public class GetNotificationOverviewInternal : IRequest<NotificationOverview>
    {
        public Guid NotificationId { get; private set; }

        public GetNotificationOverviewInternal(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
