namespace EA.Iws.Domain
{
    using System;
    using Core.Domain;
    using Utils;

    public class Address : Entity
    {
        public string Building { get; private set; }

        public string StreetOrSuburb { get; private set; }

        public string TownOrCity { get; private set; }

        public virtual string PostalCode { get; private set; }

        public virtual string Region { get; private set; }

        public Country Country { get; private set; }

        public bool IsUkAddress
        {
            get
            {
                if (this.Country == null 
                    || !this.Country.IsoAlpha2Code.Equals("GB", 
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                return true;
            }
        }

        public Address(string building, 
            string townOrCity, 
            string postalCode,
            Country country,
            string region = null,
            string streetOrSuburb = null)
        {
            Guard.ArgumentNotNull(building);
            Guard.ArgumentNotNull(townOrCity);
            Guard.ArgumentNotNull(postalCode);
            Guard.ArgumentNotNull(country);

            this.Building = building;
            this.TownOrCity = townOrCity;
            this.PostalCode = postalCode;
            this.Country = country;
            this.Region = region;
            this.StreetOrSuburb = streetOrSuburb;
        }

        private Address()
        {
        }
    }
}