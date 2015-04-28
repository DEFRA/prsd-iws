namespace EA.Iws.Api.Client.Entities
{
    public class AddressData
    {
        public string Building { get; set; }

        public string StreetOrSuburb { get; set; }

        public string TownOrCity { get; set; }

        public virtual string PostalCode { get; set; }

        public virtual string Region { get; set; }

        public string Country { get; set; }
    }
}
