namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification.Audit;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetNotificationAuditTable : IRequest<NotificationAuditTable>
    {
        public Guid NotificationId { get; private set; }

        public int PageNumber { get; private set; }

        public GetNotificationAuditTable(Guid notificationId, int pageNumber)
        {
            this.NotificationId = notificationId;
            this.PageNumber = pageNumber;
        }
    }
}
