namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.NotificationAssessment;

    public class NotificationArchiveSummaryData
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationStatus Status { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string CompanyName { get; set; }
    }
}
