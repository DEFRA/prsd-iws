namespace EA.Iws.RequestHandlers.StateOfImport
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Core.StateOfExport;
    using Core.StateOfImport;
    using Core.TransitState;
    using Core.TransportRoute;
    using DataAccess;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.StateOfImport;

    internal class GetStateOfImportWithTransportRouteDataByNotificationIdHandler : IRequestHandler<GetStateOfImportWithTransportRouteDataByNotificationId, StateOfImportWithTransportRouteData>
    {
        private readonly IwsContext context;
        private readonly IMapper mapper;
        private readonly ITransportRouteRepository transportRouteRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly ICountryRepository countryRepository;

        public GetStateOfImportWithTransportRouteDataByNotificationIdHandler(IwsContext context,
            IMapper mapper,
            ITransportRouteRepository transportRouteRepository,
            ICompetentAuthorityRepository competentAuthorityRepository,
            ICountryRepository countryRepository)
        {
            this.context = context;
            this.mapper = mapper;
            this.transportRouteRepository = transportRouteRepository;
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.countryRepository = countryRepository;
        }

        public async Task<StateOfImportWithTransportRouteData> HandleAsync(GetStateOfImportWithTransportRouteDataByNotificationId message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.Id);
            var countries = await countryRepository.GetAllHavingCompetentAuthorities();

            var data = new StateOfImportWithTransportRouteData();

            if (transportRoute != null)
            {
                data.StateOfImport = mapper.Map<StateOfImportData>(transportRoute.StateOfImport);
                data.StateOfExport = mapper.Map<StateOfExportData>(transportRoute.StateOfExport);
                data.TransitStates = transportRoute.TransitStates.Select(t => mapper.Map<TransitStateData>(t)).ToList();

                if (transportRoute.StateOfImport != null)
                {
                    var competentAuthorities =
                        await
                            competentAuthorityRepository.GetCompetentAuthorities(transportRoute.StateOfImport.Country.Id);
                    var entryPoints =
                        await
                            context.EntryOrExitPoints.Where(
                                ep => ep.Country.Id == transportRoute.StateOfImport.Country.Id).ToArrayAsync();

                    data.CompetentAuthorities = competentAuthorities.Select(ca => mapper.Map<CompetentAuthorityData>(ca)).ToArray();
                    data.EntryPoints = entryPoints.Select(e => mapper.Map<EntryOrExitPointData>(e)).ToArray();
                }
            }
            
            data.Countries = countries.Select(c => mapper.Map<CountryData>(c)).ToArray();

            return data;
        }
    }
}
