namespace EA.Iws.Requests.Notification
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification.Audit;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetNotificationAudits : IRequest<IEnumerable<NotificationAuditForDisplay>>
    {
        public GetNotificationAudits(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
