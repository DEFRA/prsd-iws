namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain;
    using Prsd.Core.Mapper;
    using Requests.Registration;

    internal class CountryMap : IMap<Country, CountryData>
    {
        public CountryData Map(Country source)
        {
            return new CountryData
            {
                Id = source.Id,
                Name = source.Name
            };
        }
    }
}
