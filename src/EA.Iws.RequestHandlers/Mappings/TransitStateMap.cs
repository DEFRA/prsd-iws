namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shared;
    using Core.TransitState;
    using Core.TransportRoute;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Requests.Registration;
    using Requests.Shared;
    using Requests.TransitState;
    using Requests.TransportRoute;

    internal class TransitStateMap : IMap<TransitState, TransitStateData>,
        IMap<IEnumerable<TransitState>, IList<TransitStateData>>
    {
        private readonly IMap<Country, CountryData> countryMap;
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMap;

        public TransitStateMap(IMap<Country, CountryData> countryMap, 
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap, 
            IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMap)
        {
            this.countryMap = countryMap;
            this.competentAuthorityMap = competentAuthorityMap;
            this.entryOrExitPointMap = entryOrExitPointMap;
        }

        public TransitStateData Map(TransitState source)
        {
            if (source == null)
            {
                return null;
            }

            return new TransitStateData
            {
                CompetentAuthority = competentAuthorityMap.Map(source.CompetentAuthority),
                Country = countryMap.Map(source.Country),
                EntryPoint = entryOrExitPointMap.Map(source.EntryPoint),
                ExitPoint = entryOrExitPointMap.Map(source.ExitPoint),
                OrdinalPosition = source.OrdinalPosition
            };
        }

        public IList<TransitStateData> Map(IEnumerable<TransitState> source)
        {
            if (source == null)
            {
                return null;
            }

            return source.Select(Map).OrderBy(ts => ts.OrdinalPosition).ToList();
        }
    }
}
