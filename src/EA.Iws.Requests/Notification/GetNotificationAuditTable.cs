namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetNotificationAuditTable
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
