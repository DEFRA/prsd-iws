namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.NotificationAssessment;

    public class NotificationApplicationSummaryData
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationStatus Status { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
