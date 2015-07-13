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

        public GetTransitStateWithTransportRouteDataByNotificationIdHandler(IwsContext context, 
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper, 
            IMap<Country, CountryData> countryMapper, 
            IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper, 
            IMap<StateOfImport, StateOfImportData> stateOfImportMapper, 
            IMap<StateOfExport, StateOfExportData> stateOfExportMapper, 
            IMap<TransitState, TransitStateData> transitStateMapper)
        {
            this.context = context;
            this.competentAuthorityMapper = competentAuthorityMapper;
            this.countryMapper = countryMapper;
            this.entryOrExitPointMapper = entryOrExitPointMapper;
            this.stateOfImportMapper = stateOfImportMapper;
            this.stateOfExportMapper = stateOfExportMapper;
            this.transitStateMapper = transitStateMapper;
        }

        public async Task<TransitStateWithTransportRouteData> HandleAsync(GetTransitStateWithTransportRouteDataByNotificationId message)
        {
            var countries = await context.Countries.ToArrayAsync();
            var notification = await context.GetNotificationApplication(message.Id);

            var data = new TransitStateWithTransportRouteData
            {
                Countries = countries.Select(countryMapper.Map).ToArray(),
                StateOfExport = stateOfExportMapper.Map(notification.StateOfExport),
                StateOfImport = stateOfImportMapper.Map(notification.StateOfImport),
                TransitStates = notification.TransitStates.Select(transitStateMapper.Map).ToArray()
            };

            var thisTransitState = notification.TransitStates.SingleOrDefault(ts => ts.Id == message.TransitStateId);
            if (thisTransitState != null)
            {
                var competentAuthorities =
                    await
                        context.CompetentAuthorities.Where(ca => ca.Country.Id == thisTransitState.Country.Id).ToArrayAsync();
                var entryPoints =
                    await
                        context.EntryOrExitPoints.Where(ep => ep.Country.Id == thisTransitState.Country.Id)
                            .ToArrayAsync();

                data.CompetentAuthorities = competentAuthorities.Select(competentAuthorityMapper.Map).ToArray();
                data.EntryOrExitPoints = entryPoints.Select(entryOrExitPointMapper.Map).ToArray();
                data.TransitStates = data.TransitStates.Where(ts => ts.Id != thisTransitState.Id).ToArray();
                data.TransitState = transitStateMapper.Map(thisTransitState);
            }

            return data;
        }
    }
}
