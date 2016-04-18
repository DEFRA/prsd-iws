namespace EA.Iws.RequestHandlers.Shared
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Shared;

    public class GetCompetentAuthoritiesByCountryHandler : IRequestHandler<GetCompetentAuthoritiesByCountry, ICollection<CompetentAuthorityData>>
    {
        private readonly IwsContext context;
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> mapper;

        public GetCompetentAuthoritiesByCountryHandler(IwsContext context, IMap<CompetentAuthority, CompetentAuthorityData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ICollection<CompetentAuthorityData>> HandleAsync(GetCompetentAuthoritiesByCountry message)
        {
            var competentAuthorities =
                await context.CompetentAuthorities.Where(ca => ca.Country.Id == message.CountryId).ToArrayAsync();

            return competentAuthorities.Select(mapper.Map).ToArray();
        }
    }
}
