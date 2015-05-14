namespace EA.Iws.Requests.Organisations
{
    using System;

    public class AddressData
    {
        public string Building { get; set; }

        public string StreetOrSuburb { get; set; }

        public string Address2 { get; set; }

        public string TownOrCity { get; set; }

        public string PostalCode { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        //public Guid CountryId { get; set; } @SCL
    }
}
