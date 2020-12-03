namespace EA.Iws.RequestHandlers.TransportRoute
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Core.TransportRoute;
    using Domain;
    using Domain.TransportRoute;
    using EA.Iws.DataAccess;
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
        private readonly IwsContext context;

        public GetCompetentAuthoritiesAndEntryPointsByCountryIdHandler(IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper,
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper,
            ICompetentAuthorityRepository competentAuthorityRepository,
            IEntryOrExitPointRepository entryOrExitPointRepository,
            IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository,
            IwsContext context)
        {
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.entryOrExitPointMapper = entryOrExitPointMapper;
            this.competentAuthorityMapper = competentAuthorityMapper;
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.intraCountryExportAllowedRepository = intraCountryExportAllowedRepository;
            this.context = context;
        }

        public async Task<CompetentAuthorityAndEntryOrExitPointData> HandleAsync(
            GetCompetentAuthoritiesAndEntryPointsByCountryId message)
        {
            IEnumerable<CompetentAuthority> competentAuthorities;

            var isUk = this.context.IsCountryUk(message.Id);

            var entryOrExitPoints = entryOrExitPointRepository.GetForCountry(message.Id);

            if (await isUk)
            {
                var ids = (await intraCountryExportAllowedRepository.GetImportCompetentAuthorities(message.NotificationUkCompetentAuthority))
                                                                    .Select(x => x.ImportCompetentAuthorityId).ToList();
                competentAuthorities = (await competentAuthorityRepository.GetByIds(ids));
            }
            else
            {
                competentAuthorities = (await competentAuthorityRepository.GetCompetentAuthorities(message.Id));
            }

            return new CompetentAuthorityAndEntryOrExitPointData
            {
                CompetentAuthorities = competentAuthorities.Select(competentAuthorityMapper.Map).ToArray(),
                EntryOrExitPoints = (await entryOrExitPoints).Select(entryOrExitPointMapper.Map).ToArray()
            };
        }
    }
}