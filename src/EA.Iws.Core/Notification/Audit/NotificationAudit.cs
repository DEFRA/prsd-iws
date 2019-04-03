namespace EA.Iws.Core.Notification.Audit
{
    using System;

    public class NotificationAudit
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }
        public string UserId { get; set; }
        public int Screen { get; set; }
        public NotificationAuditType Type { get; set; }
        public DateTimeOffset DateAdded { get; set; }
    }
}
