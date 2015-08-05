namespace EA.Iws.RequestHandlers.TransportRoute
{
    using System.Data.Entity;
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

    internal class GetCompetentAuthoritiesAndEntryOrExitPointsByCountryIdHandler :
        IRequestHandler<GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId, CompententAuthorityAndEntryOrExitPointData>
    {
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper;
        private readonly IwsContext context;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper;

        public GetCompetentAuthoritiesAndEntryOrExitPointsByCountryIdHandler(IwsContext context,
            IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper,
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper)
        {
            this.context = context;
            this.entryOrExitPointMapper = entryOrExitPointMapper;
            this.competentAuthorityMapper = competentAuthorityMapper;
        }

        public async Task<CompententAuthorityAndEntryOrExitPointData> HandleAsync(
            GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId message)
        {
            var competentAuthorities = await context.CompetentAuthorities
                                        .Where(ca => ca.Country.Id == message.Id)
                                        .OrderBy(x => x.Code)
                                        .ToArrayAsync();
            var entryOrExitPoints =
                await context.EntryOrExitPoints.Where(ep => ep.Country.Id == message.Id).ToArrayAsync();

            return new CompententAuthorityAndEntryOrExitPointData
            {
                CompetentAuthorities = competentAuthorities.Select(competentAuthorityMapper.Map).ToArray(),
                EntryOrExitPoints = entryOrExitPoints.Select(entryOrExitPointMapper.Map).ToArray()
            };
        }
    }
}