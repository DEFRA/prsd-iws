namespace EA.Iws.Core.Admin.Search
{
    using System;

    public class ImportAdvancedSearchResult
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public string Status { get; set; }

        public string Exporter { get; set; }

        public string BaselOecdCode { get; set; }

        public bool ShowShipmentSummaryLink { get; set; }
    }
}
