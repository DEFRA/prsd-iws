namespace EA.Iws.Requests.Notification
{
    using System;

    public class NotificationArchiveSummaryData
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public string Status { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string CompanyName { get; set; }

        public bool IsSelected { get; set; }

        public int? PageNumber { get; set; }
    }
}
