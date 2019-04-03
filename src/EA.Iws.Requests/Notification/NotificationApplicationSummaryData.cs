namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.NotificationAssessment;

    public class NotificationApplicationSummaryData
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationStatus Status { get; set; }

        public DateTimeOffset StatusDate { get; set; }

        public string Exporter { get; set; }

        public string Producer { get; set; }

        public string Importer { get; set; }

        public string AccessLevel { get; set; }
    }
}
