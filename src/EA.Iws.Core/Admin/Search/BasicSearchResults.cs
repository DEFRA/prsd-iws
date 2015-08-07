namespace EA.Iws.Core.Admin.Search
{
    using System;

    public class BasicSearchResult
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public string NotificationStatus { get; set; }

        public string ExporterName { get; set; }

        public string WasteType { get; set; }
    }
}
