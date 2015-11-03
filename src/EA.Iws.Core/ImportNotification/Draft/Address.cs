namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;

    public class Address
    {
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string PostalCode { get; set; }

        public string TownOrCity { get; set; }

        public Guid? CountryId { get; set; }
    }
}
