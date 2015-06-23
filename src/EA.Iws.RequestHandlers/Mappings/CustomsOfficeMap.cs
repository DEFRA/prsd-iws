namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.CustomsOffice;
    using Core.Shared;
    using Domain;
    using Domain.Notification;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;

    internal class CustomsOfficeMap : IMap<CustomsOffice, CustomsOfficeData>
    {
        private readonly IMap<Country, CountryData> countryMap;

        public CustomsOfficeMap(IMap<Country, CountryData> countryMap)
        {
            this.countryMap = countryMap;
        }

        public CustomsOfficeData Map(CustomsOffice source)
        {
            if (source == null)
            {
                return null;
            }

            return new CustomsOfficeData
            {
                Address = source.Address,
                Name = source.Name,
                Country = countryMap.Map(source.Country)
            };
        }
    }
}
