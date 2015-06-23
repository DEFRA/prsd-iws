namespace EA.Iws.Domain.TransportRoute
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class CustomsOffice : Entity
    {
        public string Name { get; private set; }

        public string Address { get; private set; }

        public Country Country { get; private set; }

        protected CustomsOffice()
        {
        }

        public CustomsOffice(string name, string address, Country country)
        {
            Guard.ArgumentNotNullOrEmpty(() => name, name);
            Guard.ArgumentNotNullOrEmpty(() => address, address);
            Guard.ArgumentNotNull(() => country, country);

            if (!country.IsEuropeanUnionMember)
            {
                throw new InvalidOperationException("The customs office must be in a European Union country. Attempted to create one in " + country.Name);
            }

            Name = name;
            Address = address;
            Country = country;
        }
    }
}
