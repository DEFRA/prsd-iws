namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Requests.Registration;
    using Requests.Shared;
    using Requests.StateOfExport;
    using Requests.TransportRoute;

    internal class StateOfExportMap : IMap<StateOfExport, StateOfExportData>
    {
        private readonly IMap<Country, CountryData> countryMap;
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMap;

        public StateOfExportMap(IMap<Country, CountryData> countryMap, 
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap, 
            IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMap)
        {
            this.countryMap = countryMap;
            this.competentAuthorityMap = competentAuthorityMap;
            this.entryOrExitPointMap = entryOrExitPointMap;
        }

        public StateOfExportData Map(StateOfExport source)
        {
            if (source == null)
            {
                return null;
            }

            return new StateOfExportData
            {
                CompetentAuthority = competentAuthorityMap.Map(source.CompetentAuthority),
                Country = countryMap.Map(source.Country),
                ExitPoint = entryOrExitPointMap.Map(source.ExitPoint)
            };
        }
    }
}
