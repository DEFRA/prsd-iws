namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using System.Threading.Tasks;
    using Core.Shared;
    using Core.StateOfExport;
    using Core.TransportRoute;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using StateOfExport = Core.ImportNotification.Draft.StateOfExport;

    internal class StateOfExportMap : IMap<StateOfExport, Domain.ImportNotification.StateOfExport>,
        IMap<Domain.ImportNotification.StateOfExport, StateOfExportData>
    {
        private readonly ICountryRepository countryRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly IMap<Country, CountryData> countryMap;
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMap;

        public StateOfExportMap(ICountryRepository countryRepository, ICompetentAuthorityRepository competentAuthorityRepository,
            IEntryOrExitPointRepository entryOrExitPointRepository,
            IMap<Country, CountryData> countryMap,
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap,
            IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMap)
        {
            this.countryRepository = countryRepository;
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.countryMap = countryMap;
            this.competentAuthorityMap = competentAuthorityMap;
            this.entryOrExitPointMap = entryOrExitPointMap;
        }

        public Domain.ImportNotification.StateOfExport Map(StateOfExport source)
        {
            return new Domain.ImportNotification.StateOfExport(source.ExitPointId.Value,
                source.CompetentAuthorityId.Value, source.CountryId.Value);
        }

        public StateOfExportData Map(Domain.ImportNotification.StateOfExport source)
        {
            if (source == null)
            {
                return null;
            }

            var country = Task.Run(() => countryRepository.GetById(source.CountryId)).Result;
            var competentAuthority = Task.Run(() => competentAuthorityRepository.GetById(source.CompetentAuthorityId)).Result;
            var exitPoint = Task.Run(() => entryOrExitPointRepository.GetById(source.ExitPointId)).Result;

            return new StateOfExportData
            {
                CompetentAuthority = competentAuthorityMap.Map(competentAuthority),
                Country = countryMap.Map(country),
                ExitPoint = entryOrExitPointMap.Map(exitPoint)
            };
        }
    }
}