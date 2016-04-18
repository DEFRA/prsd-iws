namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using System.Threading.Tasks;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class TransitStateMap : IMap<Domain.TransitState, Core.TransitState>
    {
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IEntryOrExitPointRepository pointRepository;
        private readonly ICountryRepository countryRepository;

        public TransitStateMap(ICountryRepository countryRepository,
            IEntryOrExitPointRepository pointRepository,
            ICompetentAuthorityRepository competentAuthorityRepository)
        {
            this.countryRepository = countryRepository;
            this.pointRepository = pointRepository;
            this.competentAuthorityRepository = competentAuthorityRepository;
        }

        public Core.TransitState Map(Domain.TransitState source)
        {
            var competentAuthority = Task.Run(() => competentAuthorityRepository.GetById(source.CompetentAuthorityId)).Result;

            return new Core.TransitState
            {
                CountryName = Task.Run(() => countryRepository.GetById(source.CountryId)).Result.Name,
                EntryPointName = Task.Run(() => pointRepository.GetById(source.EntryPointId)).Result.Name,
                ExitPointName = Task.Run(() => pointRepository.GetById(source.ExitPointId)).Result.Name,
                CompetentAuthorityCode = competentAuthority.Code,
                CompetentAuthorityName = competentAuthority.Name
            };
        }
    }
}