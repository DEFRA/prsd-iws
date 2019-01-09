namespace EA.Iws.Requests.Notification
{
    using EA.Iws.Core.Notification.Audit;
    using EA.Prsd.Core.Mediator;
    using System;

    public class CreateNotificationAudit : IRequest<bool>
    {
        public Guid NotificationId { get; set; }
        public string UserId { get; set; }
        public int Screen { get; set; }
        public NotificationAuditType Type { get; set; }
        public DateTimeOffset DateAdded { get; set; }
    }
}
