namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Linq;
    using Core.CustomsOffice;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Requests.CustomsOffice;

    internal class CustomsOfficeExitMap : IMap<NotificationApplication, ExitCustomsOfficeAddData>
    {
        private readonly IwsContext context;
        private readonly IMap<Country, CountryData> countryMap;
        private readonly IMap<CustomsOffice, CustomsOfficeData> customsOfficeMap;

        public CustomsOfficeExitMap(IwsContext context, IMap<Country, CountryData> countryMap,
            IMap<CustomsOffice, CustomsOfficeData> customsOfficeMap)
        {
            this.context = context;
            this.countryMap = countryMap;
            this.customsOfficeMap = customsOfficeMap;
        }
 
        public ExitCustomsOfficeAddData Map(NotificationApplication source)
        {
            var countries = context.Countries.Where(c => c.IsEuropeanUnionMember).OrderBy(c => c.Name).ToArray();
 
            return new ExitCustomsOfficeAddData
            {
                Countries = countries.Select(countryMap.Map).ToArray(),
                CustomsOffices = source.GetCustomsOfficesRequired(),
                CustomsOfficeData = customsOfficeMap.Map(source.ExitCustomsOffice)
            };
        }
    }
}
