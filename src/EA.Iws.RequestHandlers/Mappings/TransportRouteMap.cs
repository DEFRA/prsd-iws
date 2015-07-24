namespace EA.Iws.RequestHandlers.Mappings
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
    using Domain.NotificationApplication;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Requests.StateOfExport;

    internal class TransportRouteMap : IMap<NotificationApplication, StateOfExportWithTransportRouteData>
    {
        private readonly IwsContext context;
        private readonly IMap<StateOfImport, StateOfImportData> stateOfImportMapper;
        private readonly IMap<StateOfExport, StateOfExportData> stateOfExportMapper;
        private readonly IMap<IEnumerable<TransitState>, IList<TransitStateData>> transitStateMapper;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper;
        private readonly IMap<Country, CountryData> countryMapper;
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper;

        public TransportRouteMap(IwsContext context,
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

        public StateOfExportWithTransportRouteData Map(NotificationApplication source)
        {
            var countries = context.Countries.OrderBy(c => c.Name).ToArray();

            var data = new StateOfExportWithTransportRouteData
            {
                StateOfImport = stateOfImportMapper.Map(source.StateOfImport),
                StateOfExport = stateOfExportMapper.Map(source.StateOfExport),
                Countries = countries.Select(countryMapper.Map).ToArray(),
                TransitStates = transitStateMapper.Map(source.TransitStates)
            };

            if (source.StateOfExport != null)
            {
                var competentAuthorities = context.CompetentAuthorities.Where(ca => ca.Country.Id == source.StateOfExport.Country.Id).ToArray();
                var entryPoints = context.EntryOrExitPoints.Where(ep => ep.Country.Id == source.StateOfExport.Country.Id).ToArray();

                data.CompetentAuthorities = competentAuthorities.Select(competentAuthorityMapper.Map).ToArray();
                data.ExitPoints = entryPoints.Select(entryOrExitPointMapper.Map).ToArray();
            }

            return data;
        }
    }
}
