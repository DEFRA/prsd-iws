namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain;
    using Prsd.Core.Mapper;
    using Requests.Shared;

    public class AddressMap : IMap<Address, AddressData>
    {
        public AddressData Map(Address source)
        {
            return new AddressData
            {
                Building = source.Building,
                StreetOrSuburb = source.Address1,
                Address2 = source.Address2,
                TownOrCity = source.TownOrCity,
                Region = source.Region,
                PostalCode = source.PostalCode,
                CountryName = source.Country
            };
        }
    }
}
