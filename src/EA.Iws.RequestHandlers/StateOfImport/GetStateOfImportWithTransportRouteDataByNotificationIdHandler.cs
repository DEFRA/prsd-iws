namespace EA.Iws.RequestHandlers.StateOfImport
{
    using System.Collections.Generic;
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
    using StateOfImport = Domain.TransportRoute.StateOfImport;

    internal class GetStateOfImportWithTransportRouteDataByNotificationIdHandler : IRequestHandler<GetStateOfImportWithTransportRouteDataByNotificationId, StateOfImportWithTransportRouteData>
    {
        private readonly IwsContext context;
        private readonly IMap<StateOfImport, StateOfImportData> stateOfImportMapper;
        private readonly IMap<StateOfExport, StateOfExportData> stateOfExportMapper;
        private readonly IMap<IEnumerable<TransitState>, IList<TransitStateData>> transitStateMapper;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper;
        private readonly IMap<Country, CountryData> countryMapper;
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper;

        public GetStateOfImportWithTransportRouteDataByNotificationIdHandler(IwsContext context, 
            IMap<StateOfImport, StateOfImportData> stateOfImportMapper, 
            IMap<StateOfExport, StateOfExportData> stateOfExportMapper,
            IMap<IEnumerable<TransitState>, IList<TransitStateData>> transitStateMapper, 
            IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper, 
            IMap<Country, CountryData> countryMapper, 
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper)
        {
            this.context = context;
            this.stateOfImportMapper = stateOfImportMapper;
            this.stateOfExportMapper = stateOfExportMapper;
            this.transitStateMapper = transitStateMapper;
            this.entryOrExitPointMapper = entryOrExitPointMapper;
            this.countryMapper = countryMapper;
            this.competentAuthorityMapper = competentAuthorityMapper;
        }

        public async Task<StateOfImportWithTransportRouteData> HandleAsync(GetStateOfImportWithTransportRouteDataByNotificationId message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.Id);
            var countries = await context.Countries.OrderBy(c => c.Name).ToArrayAsync();

            var data = new StateOfImportWithTransportRouteData
            {
                StateOfImport = stateOfImportMapper.Map(notification.StateOfImport),
                StateOfExport = stateOfExportMapper.Map(notification.StateOfExport),
                Countries = countries.Select(countryMapper.Map).ToArray(),
                TransitStates = transitStateMapper.Map(notification.TransitStates)
            };

            if (notification.StateOfImport != null)
            {
                var competentAuthorities =
                    await
                        context.CompetentAuthorities.Where(ca => ca.Country.Id == notification.StateOfImport.Country.Id).ToArrayAsync();
                var entryPoints =
                    await
                        context.EntryOrExitPoints.Where(ep => ep.Country.Id == notification.StateOfImport.Country.Id)
                            .ToArrayAsync();

                data.CompetentAuthorities = competentAuthorities.Select(competentAuthorityMapper.Map).ToArray();
                data.EntryPoints = entryPoints.Select(entryOrExitPointMapper.Map).ToArray();
            }

            return data;
        }
    }
}
