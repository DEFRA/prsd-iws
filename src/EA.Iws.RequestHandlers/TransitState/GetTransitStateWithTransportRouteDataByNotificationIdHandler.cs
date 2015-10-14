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
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper;
        private readonly IMap<Country, CountryData> countryMapper;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper;
        private readonly IMap<StateOfImport, StateOfImportData> stateOfImportMapper;
        private readonly IMap<StateOfExport, StateOfExportData> stateOfExportMapper;
        private readonly IMap<TransitState, TransitStateData> transitStateMapper;
        private readonly ITransportRouteRepository transportRouteRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;

        public GetTransitStateWithTransportRouteDataByNotificationIdHandler(IwsContext context,
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper,
            IMap<Country, CountryData> countryMapper,
            IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper,
            IMap<StateOfImport, StateOfImportData> stateOfImportMapper,
            IMap<StateOfExport, StateOfExportData> stateOfExportMapper,
            IMap<TransitState, TransitStateData> transitStateMapper,
            ITransportRouteRepository transportRouteRepository,
            ICompetentAuthorityRepository competentAuthorityRepository)
        {
            this.context = context;
            this.competentAuthorityMapper = competentAuthorityMapper;
            this.countryMapper = countryMapper;
            this.entryOrExitPointMapper = entryOrExitPointMapper;
            this.stateOfImportMapper = stateOfImportMapper;
            this.stateOfExportMapper = stateOfExportMapper;
            this.transitStateMapper = transitStateMapper;
            this.transportRouteRepository = transportRouteRepository;
            this.competentAuthorityRepository = competentAuthorityRepository;
        }

        public async Task<TransitStateWithTransportRouteData> HandleAsync(GetTransitStateWithTransportRouteDataByNotificationId message)
        {
            var countries = await context.Countries.ToArrayAsync();

            var transportRoute = await transportRouteRepository.GetByNotificationId(message.Id);

            if (transportRoute == null)
            {
                return new TransitStateWithTransportRouteData
                {
                    Countries = countries.Select(countryMapper.Map).ToArray()
                };
            }

            var data = new TransitStateWithTransportRouteData
            {
                Countries = countries.Select(countryMapper.Map).ToArray(),
                StateOfExport = stateOfExportMapper.Map(transportRoute.StateOfExport),
                StateOfImport = stateOfImportMapper.Map(transportRoute.StateOfImport),
                TransitStates = transportRoute.TransitStates.Select(transitStateMapper.Map).ToArray()
            };

            var thisTransitState = transportRoute.TransitStates.SingleOrDefault(ts => ts.Id == message.TransitStateId);
            if (thisTransitState != null)
            {
                var competentAuthorities =
                    await competentAuthorityRepository.GetTransitAuthorities(thisTransitState.Country.Id);
                var entryPoints = await context.EntryOrExitPoints.Where(ep => ep.Country.Id == thisTransitState.Country.Id).ToArrayAsync();

                data.CompetentAuthorities = competentAuthorities.Select(competentAuthorityMapper.Map).ToArray();
                data.EntryOrExitPoints = entryPoints.Select(entryOrExitPointMapper.Map).ToArray();
                data.TransitStates = data.TransitStates.Where(ts => ts.Id != thisTransitState.Id).ToArray();
                data.TransitState = transitStateMapper.Map(thisTransitState);
            }

            return data;
        }
    }
}
