namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Address = Core.ImportNotification.Draft.Address;

    internal class AddressMap : IMap<Address, Domain.ImportNotification.Address>
    {
        private readonly AddressBuilder addressBuilder;

        public AddressMap(AddressBuilder addressBuilder)
        {
            this.addressBuilder = addressBuilder;
        }

        public Domain.ImportNotification.Address Map(Address source)
        {
            var builder = addressBuilder.Create(source.AddressLine1, source.TownOrCity, source.CountryId.Value);

            if (!string.IsNullOrWhiteSpace(source.PostalCode))
            {
                builder = builder.WithPostalCode(source.PostalCode);
            }

            if (!string.IsNullOrWhiteSpace(source.AddressLine2))
            {
                builder = builder.WithAddressLine2(source.AddressLine2);
            }

            return builder.ToAddress();
        }
    }
}