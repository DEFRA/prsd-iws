namespace EA.Iws.Core.Notification.Audit
{
    using System;

    public class NotificationAuditForDisplay
    {
        public string UserName { get; set; }

        public string ScreenName { get; set; }

        public string AuditType { get; set; }

        public DateTimeOffset DateAdded { get; set; }
    }
}
