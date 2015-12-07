namespace EA.Iws.Domain.ImportNotification
{
    using System;

    public class Address
    {
        internal Address(string addressLine1, string addressLine2, string townOrCity, string postalCode, Guid countryId)
        {
            Address1 = addressLine1;
            Address2 = addressLine2;
            TownOrCity = townOrCity;
            PostalCode = postalCode;
            CountryId = countryId;
        }

        public string Address1 { get; private set; }

        public string Address2 { get; private set; }

        public string PostalCode { get; private set; }

        public string TownOrCity { get; private set; }

        public Guid CountryId { get; private set; }
    }
}