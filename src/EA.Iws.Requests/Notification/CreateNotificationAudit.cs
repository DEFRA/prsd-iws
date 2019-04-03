namespace EA.Iws.Requests.Notification
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification.Audit;
    using Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class CreateNotificationAudit : IRequest<bool>
    {
        public Guid NotificationId { get; set; }
        public string UserId { get; set; }
        public NotificationAuditScreenType Screen { get; set; }
        public NotificationAuditType Type { get; set; }
        public DateTimeOffset DateAdded { get; set; }
    }
}
