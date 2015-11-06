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

    internal class GetUnitedKingdomCompetentAuthoritiesAndEntryOrExitPointsHandler 
        : IRequestHandler<GetUnitedKingdomCompetentAuthoritiesAndEntryOrExitPoints, CompetentAuthorityAndEntryOrExitPointData>
    {
        private readonly ICountryRepository countryRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly IMapper mapper;

        public GetUnitedKingdomCompetentAuthoritiesAndEntryOrExitPointsHandler(ICountryRepository countryRepository, 
            ICompetentAuthorityRepository competentAuthorityRepository,
            IEntryOrExitPointRepository entryOrExitPointRepository, 
            IMapper mapper)
        {
            this.countryRepository = countryRepository;
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.mapper = mapper;
        }

        public async Task<CompetentAuthorityAndEntryOrExitPointData> HandleAsync(GetUnitedKingdomCompetentAuthoritiesAndEntryOrExitPoints message)
        {
            var countryId = await countryRepository.GetUnitedKingdomId();

            var competentAuthorities = await competentAuthorityRepository.GetCompetentAuthorities(countryId);

            var entryOrExitPoints = await entryOrExitPointRepository.GetForCountry(countryId);

            return new CompetentAuthorityAndEntryOrExitPointData
            {
                CompetentAuthorities =
                    competentAuthorities.Select(ca => mapper.Map<CompetentAuthorityData>(ca)).ToArray(),
                EntryOrExitPoints = entryOrExitPoints.Select(eep => mapper.Map<EntryOrExitPointData>(eep)).ToArray()
            };
        }
    }
}
