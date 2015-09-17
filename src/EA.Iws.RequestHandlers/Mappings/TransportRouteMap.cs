namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shared;
    using Core.StateOfExport;
    using Core.StateOfImport;
    using Core.TransitState;
    using Core.TransportRoute;
    using DataAccess;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Requests.StateOfExport;

    internal class TransportRouteMap : IMap<TransportRoute, StateOfExportWithTransportRouteData>
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

        public StateOfExportWithTransportRouteData Map(TransportRoute source)
        {
            var countries = context.Countries.OrderBy(c => c.Name).ToArray();
            var notification = context.NotificationApplications.Single(n => n.Id == source.NotificationId);

            var data = new StateOfExportWithTransportRouteData
            {
                StateOfImport = stateOfImportMapper.Map(source.StateOfImport),
                StateOfExport = stateOfExportMapper.Map(source.StateOfExport),
                Countries = countries.Select(countryMapper.Map).ToArray(),
                TransitStates = transitStateMapper.Map(source.TransitStates)
            };

            if (source.StateOfExport == null)
            {
                data.StateOfExport = new StateOfExportData();

                var ukcompAuth = context.UnitedKingdomCompetentAuthorities.Single(ca => ca.Id == notification.CompetentAuthority.Value);

                data.StateOfExport.Country = countryMapper.Map(countries.Single(c => c.Name == ukcompAuth.CountryName));

                data.StateOfExport.CompetentAuthority = competentAuthorityMapper.Map(context.CompetentAuthorities.Single(ca => ca.Id == ukcompAuth.CompetentAuthority.Id));
            }

            var entryPoints = context.EntryOrExitPoints.Where(ep => ep.Country.Id == data.StateOfExport.Country.Id).ToArray();
            data.ExitPoints = entryPoints.Select(entryOrExitPointMapper.Map).ToArray();
            
            return data;
        }
    }
}
