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

    internal class GetCompetentAuthoritiesAndEntryPointsByCountryIdHandler :
        IRequestHandler<GetCompetentAuthoritiesAndEntryPointsByCountryId, CompetentAuthorityAndEntryOrExitPointData>
    {
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper;

        public GetCompetentAuthoritiesAndEntryPointsByCountryIdHandler(IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper,
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper,
            ICompetentAuthorityRepository competentAuthorityRepository,
            IEntryOrExitPointRepository entryOrExitPointRepository,
            IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository)
        {
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.entryOrExitPointMapper = entryOrExitPointMapper;
            this.competentAuthorityMapper = competentAuthorityMapper;
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.intraCountryExportAllowedRepository = intraCountryExportAllowedRepository;
        }

        public async Task<CompetentAuthorityAndEntryOrExitPointData> HandleAsync(
            GetCompetentAuthoritiesAndEntryPointsByCountryId message)
        {
            var competentAuthorities = (await competentAuthorityRepository.GetCompetentAuthorities(message.Id));

            if (message.ExitPointCompetentAuthorityId.HasValue && competentAuthorities.Any(x => x.Id == message.ExitPointCompetentAuthorityId.Value))
            {
                // see if there are any restrictions on this export CA
                var allowed = (await intraCountryExportAllowedRepository.GetImportCompetentAuthorities(message.ExitPointCompetentAuthorityId.Value)).Select(x => x.ImportCompetentAuthorityId).ToList();

                if (allowed.Any())
                {
                    competentAuthorities = competentAuthorities.Where(x => allowed.Contains(x.Id));
                }
            }

            var entryOrExitPoints =
                await entryOrExitPointRepository.GetForCountry(message.Id);

            return new CompetentAuthorityAndEntryOrExitPointData
            {
                CompetentAuthorities = competentAuthorities.Select(competentAuthorityMapper.Map).ToArray(),
                EntryOrExitPoints = entryOrExitPoints.Select(entryOrExitPointMapper.Map).ToArray()
            };
        }
    }
}