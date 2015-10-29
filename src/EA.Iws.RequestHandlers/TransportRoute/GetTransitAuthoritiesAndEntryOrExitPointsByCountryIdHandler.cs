namespace EA.Iws.RequestHandlers.TransportRoute
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Core.TransportRoute;
    using DataAccess;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.TransportRoute;

    internal class GetTransitAuthoritiesAndEntryOrExitPointsByCountryIdHandler :
        IRequestHandler<GetTransitAuthoritiesAndEntryOrExitPointsByCountryId, CompententAuthorityAndEntryOrExitPointData>
    {
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper;

        public GetTransitAuthoritiesAndEntryOrExitPointsByCountryIdHandler(IwsContext context,
            IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper,
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper,
            IEntryOrExitPointRepository entryOrExitPointRepository,
            ICompetentAuthorityRepository competentAuthorityRepository)
        {
            this.entryOrExitPointMapper = entryOrExitPointMapper;
            this.competentAuthorityMapper = competentAuthorityMapper;
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.competentAuthorityRepository = competentAuthorityRepository;
        }

        public async Task<CompententAuthorityAndEntryOrExitPointData> HandleAsync(
            GetTransitAuthoritiesAndEntryOrExitPointsByCountryId message)
        {
            var competentAuthorities = (await competentAuthorityRepository.GetTransitAuthorities(message.Id));

            var entryOrExitPoints = await entryOrExitPointRepository.GetForCountry(message.Id);

            return new CompententAuthorityAndEntryOrExitPointData
            {
                CompetentAuthorities = competentAuthorities.Select(competentAuthorityMapper.Map).ToArray(),
                EntryOrExitPoints = entryOrExitPoints.Select(entryOrExitPointMapper.Map).ToArray()
            };
        }
    }
}