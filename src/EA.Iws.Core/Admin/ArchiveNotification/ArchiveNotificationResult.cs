namespace EA.Iws.Core.Admin.ArchiveNotification
{
    using System;

    public class ArchiveNotificationResult
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public string Status { get; set; }

        public DateTime DateGenerated { get; set; }

        public string CompanyName { get; set; }
    }
}
