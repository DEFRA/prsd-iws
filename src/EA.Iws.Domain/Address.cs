namespace EA.Iws.Domain
{
    using System;
    using Core.Domain;
    using Utils;

    public class Address : Entity
    {
        public string Building { get; private set; }

        public string Address1 { get; private set; }

        public virtual string Address2 { get; private set; }

        public string TownOrCity { get; private set; }

        public virtual string PostalCode { get; private set; }

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
            string address1,
            string townOrCity, 
            string postalCode,
            Country country,
            string address2 = null)
        {
            Guard.ArgumentNotNull(building);
            Guard.ArgumentNotNull(townOrCity);
            Guard.ArgumentNotNull(postalCode);
            Guard.ArgumentNotNull(country);
            Guard.ArgumentNotNull(address1);

            this.Building = building;
            this.TownOrCity = townOrCity;
            this.PostalCode = postalCode;
            this.Country = country;
            this.Address2 = address2;
            this.Address1 = address1;
        }

        private Address()
        {
        }
    }
}