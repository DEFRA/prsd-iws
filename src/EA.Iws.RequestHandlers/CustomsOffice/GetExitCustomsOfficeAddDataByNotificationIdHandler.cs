namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.CustomsOffice;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;
    using CustomsOffice = Domain.TransportRoute.CustomsOffice;

    internal class GetExitCustomsOfficeAddDataByNotificationIdHandler : IRequestHandler<GetExitCustomsOfficeAddDataByNotificationId, ExitCustomsOfficeAddData>
    {
        private readonly IwsContext context;
        private readonly IMap<Country, CountryData> countryMap;
        private readonly IMap<CustomsOffice, CustomsOfficeData> customsOfficeMap;

        public GetExitCustomsOfficeAddDataByNotificationIdHandler(IwsContext context, 
            IMap<Country, CountryData> countryMap, 
            IMap<CustomsOffice, CustomsOfficeData> customsOfficeMap)
        {
            this.context = context;
            this.countryMap = countryMap;
            this.customsOfficeMap = customsOfficeMap;
        }

        public async Task<ExitCustomsOfficeAddData> HandleAsync(GetExitCustomsOfficeAddDataByNotificationId message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.Id);

            var countries = await context.Countries.Where(c => c.IsEuropeanUnionMember).OrderBy(c => c.Name).ToArrayAsync();

            return new ExitCustomsOfficeAddData
            {
                Countries = countries.Select(countryMap.Map).ToArray(),
                CustomsOffices = notification.GetCustomsOfficesRequired(),
                CustomsOfficeData = customsOfficeMap.Map(notification.ExitCustomsOffice)
            };
        }
    }
}
