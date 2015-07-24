namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Linq;
    using Core.CustomsOffice;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Requests.CustomsOffice;

    internal class CustomsOfficeEntryMap : IMap<NotificationApplication, EntryCustomsOfficeAddData>
    {
        private readonly IwsContext context;
        private readonly IMap<Country, CountryData> countryMap;
        private readonly IMap<CustomsOffice, CustomsOfficeData> customsOfficeMap;

        public CustomsOfficeEntryMap(IwsContext context, IMap<Country, CountryData> countryMap,
            IMap<CustomsOffice, CustomsOfficeData> customsOfficeMap)
        {
            this.context = context;
            this.countryMap = countryMap;
            this.customsOfficeMap = customsOfficeMap;
        }
 
        public EntryCustomsOfficeAddData Map(NotificationApplication source)
        {
            var countries = context.Countries.Where(c => c.IsEuropeanUnionMember).OrderBy(c => c.Name).ToArray();
 
            return new EntryCustomsOfficeAddData
            {
                Countries = countries.Select(countryMap.Map).ToArray(),
                CustomsOfficesRequired = source.GetCustomsOfficesRequired(),
                CustomsOfficeData = customsOfficeMap.Map(source.EntryCustomsOffice)
            };
        }
    }
}
