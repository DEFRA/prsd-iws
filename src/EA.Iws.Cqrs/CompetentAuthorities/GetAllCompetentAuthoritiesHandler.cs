namespace EA.Iws.Cqrs.CompetentAuthorities
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;

    public class GetAllCompetentAuthoritiesHandler : IRequestHandler<GetAllCompetentAuthorities, IList<CompetentAuthority>>
    {
        private readonly IwsContext context;

        public GetAllCompetentAuthoritiesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IList<CompetentAuthority>> HandleAsync(GetAllCompetentAuthorities query)
        {
            var competentAuthorities = await context.CompetentAuthorities.ToListAsync();

            return competentAuthorities;
        }
    }
}
