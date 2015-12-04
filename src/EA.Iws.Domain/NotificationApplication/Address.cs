namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Prsd.Core;

    public class Address
    {
        public Address(string address1, string address2, string townOrCity, string region,
            string postalCode, string country)
        {
            Guard.ArgumentNotNullOrEmpty(() => townOrCity, townOrCity);
            Guard.ArgumentNotNullOrEmpty(() => country, country);
            Guard.ArgumentNotNullOrEmpty(() => address1, address1);

            Country = country;

            if (IsUkAddress && string.IsNullOrWhiteSpace(postalCode))
            {
                throw new InvalidOperationException("Postal code cannot be null for UK addresses.");
            }

            TownOrCity = townOrCity;
            PostalCode = postalCode;
            Address2 = address2;
            Address1 = address1;
            Region = region;
        }

        protected Address()
        {
        }

        public string Address1 { get; private set; }

        public string Address2 { get; private set; }

        public string TownOrCity { get; private set; }

        public string Region { get; private set; }

        public string PostalCode { get; private set; }

        public string Country { get; private set; }

        public bool IsUkAddress
        {
            get
            {
                if (!Country.Equals("United Kingdom",
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                return true;
            }
        }
    }
}