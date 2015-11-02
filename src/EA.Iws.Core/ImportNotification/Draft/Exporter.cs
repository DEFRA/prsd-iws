namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;

    public class Exporter
    {
        public string BusinessName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string PostalCode { get; set; }

        public string TownOrCity { get; set; }

        public string ContactName { get; set; }

        public string Telephone { get; set; }

        public Guid? CountryId { get; set; }

        public string Email { get; set; }
    }
}
