namespace EA.Iws.Core.Admin.Search
{
    using System;

    public class AdvancedSearchCriteria
    {
        public string EwcCode { get; set; }

        public string ProducerName { get; set; }

        public string ImporterName { get; set; }

        public string ImportCountryName { get; set; }

        public Guid? LocalAreaId { get; set; }

        public DateTime? ConsentValidFromStart { get; set; }

        public DateTime? ConsentValidFromEnd { get; set; }

        public DateTime? ConsentValidToStart { get; set; }

        public DateTime? ConsentValidToEnd { get; set; }

        public string ExporterName { get; set; }

        public string BaselOecdCode { get; set; }

        public string FacilityName { get; set; }

        public string ExitPointName { get; set; }

        public string EntryPointName { get; set; }

        public DateTime? NotificationReceivedStart { get; set; }

        public DateTime? NotificationReceivedEnd { get; set; }
    }
}