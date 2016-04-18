namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Prsd.Core.Mapper;

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
