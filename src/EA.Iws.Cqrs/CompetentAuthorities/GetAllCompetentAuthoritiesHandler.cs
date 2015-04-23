namespace EA.Iws.Cqrs.CompetentAuthorities
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Cqrs;
    using DataAccess;
    using Domain;

    public class GetAllCompetentAuthoritiesHandler : IQueryHandler<GetAllCompetentAuthorities, IList<CompetentAuthority>>
    {
        private readonly IwsContext context;

        public GetAllCompetentAuthoritiesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IList<CompetentAuthority>> ExecuteAsync(GetAllCompetentAuthorities query)
        {
            var competentAuthorities = await context.CompetentAuthorities.ToListAsync();

            return competentAuthorities;
        }
    }
}
