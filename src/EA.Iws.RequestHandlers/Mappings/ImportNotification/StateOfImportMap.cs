namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using System.Threading.Tasks;
    using Core.Shared;
    using Core.StateOfImport;
    using Core.TransportRoute;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using StateOfImport = Core.ImportNotification.Draft.StateOfImport;

    internal class StateOfImportMap : IMap<StateOfImport, Domain.ImportNotification.StateOfImport>,
        IMap<Domain.ImportNotification.StateOfImport, StateOfImportData>
    {
        private readonly ICountryRepository countryRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly IMap<Country, CountryData> countryMap;
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMap;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMap;

        public StateOfImportMap(ICountryRepository countryRepository, ICompetentAuthorityRepository competentAuthorityRepository,
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

        public Domain.ImportNotification.StateOfImport Map(StateOfImport source)
        {
            return new Domain.ImportNotification.StateOfImport(source.EntryPointId.Value,
                source.CompetentAuthorityId.Value);
        }

        public StateOfImportData Map(Domain.ImportNotification.StateOfImport source)
        {
            if (source == null)
            {
                return null;
            }

            var countryId = Task.Run(() => countryRepository.GetUnitedKingdomId()).Result;
            var country = Task.Run(() => countryRepository.GetById(countryId)).Result;
            var competentAuthority = Task.Run(() => competentAuthorityRepository.GetById(source.CompetentAuthorityId)).Result;
            var entryPoint = Task.Run(() => entryOrExitPointRepository.GetById(source.EntryPointId)).Result;

            return new StateOfImportData
            {
                CompetentAuthority = competentAuthorityMap.Map(competentAuthority),
                Country = countryMap.Map(country),
                EntryPoint = entryOrExitPointMap.Map(entryPoint)
            };
        }
    }
}