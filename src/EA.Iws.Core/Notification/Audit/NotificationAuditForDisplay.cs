namespace EA.Iws.Core.Notification.Audit
{
    using System;

    public class NotificationAuditForDisplay
    {
        public NotificationAuditForDisplay()
        {
        }

        public NotificationAuditForDisplay(string userName, string screenName, string auditType, DateTimeOffset dateAdded)
        {
            this.UserName = userName;
            this.ScreenName = screenName;
            this.AuditType = auditType;
            this.DateAdded = dateAdded;
        }

        public string UserName { get; set; }

        public string ScreenName { get; set; }

        public string AuditType { get; set; }

        public DateTimeOffset DateAdded { get; set; }
    }
}
