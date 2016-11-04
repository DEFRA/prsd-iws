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
    }
}