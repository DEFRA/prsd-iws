namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Core.StateOfImport;
    using Core.TransportRoute;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Requests.Registration;
    using Requests.Shared;
    using Requests.StateOfImport;
    using Requests.TransportRoute;

    internal class StateOfImportMap : IMap<StateOfImport, StateOfImportData>
    {
        private readonly IMap<Country, CountryData> countryMap;
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMap;

        public StateOfImportMap(IMap<Country, CountryData> countryMap, 
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap, 
            IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMap)
        {
            this.countryMap = countryMap;
            this.competentAuthorityMap = competentAuthorityMap;
            this.entryOrExitPointMap = entryOrExitPointMap;
        }

        public StateOfImportData Map(StateOfImport source)
        {
            if (source == null)
            {
                return null;
            }

            return new StateOfImportData
            {
                CompetentAuthority = competentAuthorityMap.Map(source.CompetentAuthority),
                Country = countryMap.Map(source.Country),
                EntryPoint = entryOrExitPointMap.Map(source.EntryPoint)
            };
        }
    }
}
