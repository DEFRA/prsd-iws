namespace EA.Iws.RequestHandlers.TransportRoute
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Core.TransportRoute;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.TransportRoute;

    internal class GetCompetentAuthoritiesAndEntryOrExitPointsByCountryIdHandler :
        IRequestHandler<GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId, CompententAuthorityAndEntryOrExitPointData>
    {
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper;

        public GetCompetentAuthoritiesAndEntryOrExitPointsByCountryIdHandler(IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper,
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper,
            ICompetentAuthorityRepository competentAuthorityRepository,
            IEntryOrExitPointRepository entryOrExitPointRepository)
        {
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.entryOrExitPointMapper = entryOrExitPointMapper;
            this.competentAuthorityMapper = competentAuthorityMapper;
            this.competentAuthorityRepository = competentAuthorityRepository;
        }

        public async Task<CompententAuthorityAndEntryOrExitPointData> HandleAsync(
            GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId message)
        {
            var competentAuthorities = (await competentAuthorityRepository.GetCompetentAuthorities(message.Id));

            var entryOrExitPoints =
                await entryOrExitPointRepository.GetForCountry(message.Id);

            return new CompententAuthorityAndEntryOrExitPointData
            {
                CompetentAuthorities = competentAuthorities.Select(competentAuthorityMapper.Map).ToArray(),
                EntryOrExitPoints = entryOrExitPoints.Select(entryOrExitPointMapper.Map).ToArray()
            };
        }
    }
}