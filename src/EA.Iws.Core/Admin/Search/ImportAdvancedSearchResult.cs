namespace EA.Iws.Core.Admin.Search
{
    using System;
    using ImportNotificationAssessment;

    public class ImportAdvancedSearchResult
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public ImportNotificationStatus Status { get; set; }

        public string Exporter { get; set; }

        public string BaselOecdCode { get; set; }
    }
}
