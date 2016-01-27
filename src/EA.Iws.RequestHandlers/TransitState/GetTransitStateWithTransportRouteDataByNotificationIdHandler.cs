namespace EA.Iws.RequestHandlers.TransitState
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
    using Requests.TransitState;

    internal class GetTransitStateWithTransportRouteDataByNotificationIdHandler : IRequestHandler<GetTransitStateWithTransportRouteDataByNotificationId, TransitStateWithTransportRouteData>
    {
        private readonly IwsContext context;
        private readonly IMapper mapper;
        private readonly ITransportRouteRepository transportRouteRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly ICountryRepository countryRepository;

        public GetTransitStateWithTransportRouteDataByNotificationIdHandler(IwsContext context,
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

        public async Task<TransitStateWithTransportRouteData> HandleAsync(GetTransitStateWithTransportRouteDataByNotificationId message)
        {
            var countries = (await countryRepository.GetAllHavingCompetentAuthorities())
                .Select(c => mapper.Map<CountryData>(c))
                .ToArray();

            var transportRoute = await transportRouteRepository.GetByNotificationId(message.Id);

            if (transportRoute == null)
            {
                return new TransitStateWithTransportRouteData
                {
                    Countries = countries
                };
            }

            var data = new TransitStateWithTransportRouteData
            {
                Countries = countries,
                StateOfExport = mapper.Map<StateOfExportData>(transportRoute.StateOfExport),
                StateOfImport = mapper.Map<StateOfImportData>(transportRoute.StateOfImport),
                TransitStates = transportRoute.TransitStates.Select(t => mapper.Map<TransitStateData>(t)).ToArray()
            };

            var thisTransitState = transportRoute.TransitStates.SingleOrDefault(ts => ts.Id == message.TransitStateId);
            if (thisTransitState != null)
            {
                var competentAuthorities =
                    await competentAuthorityRepository.GetTransitAuthorities(thisTransitState.Country.Id);
                var entryPoints = await context.EntryOrExitPoints.Where(ep => ep.Country.Id == thisTransitState.Country.Id).ToArrayAsync();

                data.CompetentAuthorities = competentAuthorities.Select(ca => mapper.Map<CompetentAuthorityData>(ca)).ToArray();
                data.EntryOrExitPoints = entryPoints.Select(e => mapper.Map<EntryOrExitPointData>(e)).ToArray();
                data.TransitStates = data.TransitStates.Where(ts => ts.Id != thisTransitState.Id).ToArray();
                data.TransitState = mapper.Map<TransitStateData>(thisTransitState);
            }

            return data;
        }
    }
}
