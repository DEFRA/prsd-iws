namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using Prsd.Core;

    public class AddressBuilder
    {
        private readonly ICountryRepository countryRepository;

        private string address1;
        private string address2;
        private string townOrCity;
        private string postalCode;
        private Guid countryId;

        public AddressBuilder(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public AddressBuilder Create(string addressLine1, string townOrCity, Guid countryId)
        {
            Guard.ArgumentNotNullOrEmpty(() => addressLine1, addressLine1);
            Guard.ArgumentNotNullOrEmpty(() => townOrCity, townOrCity);
            Guard.ArgumentNotDefaultValue(() => countryId, countryId);

            this.address1 = addressLine1;
            this.townOrCity = townOrCity;
            this.countryId = countryId;

            this.address2 = null;
            this.postalCode = null;

            return this;
        }

        public AddressBuilder WithAddressLine2(string addressLine2)
        {
            Guard.ArgumentNotNullOrEmpty(() => addressLine2, addressLine2);

            this.address2 = addressLine2;

            return this;
        }

        public AddressBuilder WithPostalCode(string postalCode)
        {
            Guard.ArgumentNotNullOrEmpty(() => postalCode, postalCode);

            this.postalCode = postalCode;

            return this;
        }

        public Address ToAddress()
        {
            var country = Task.Run(() => countryRepository.GetById(countryId)).Result;

            if (country.Name.Equals("United Kingdom", StringComparison.InvariantCultureIgnoreCase)
                && string.IsNullOrWhiteSpace(postalCode))
            {
                throw new InvalidOperationException("Postal code cannot be null for UK addresses.");
            }

            return new Address(address1, address2, townOrCity, postalCode, countryId);
        }
    }
}