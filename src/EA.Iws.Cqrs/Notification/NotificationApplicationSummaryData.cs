namespace EA.Iws.Cqrs.Notification
{
    using System;

    public class NotificationApplicationSummaryData
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
