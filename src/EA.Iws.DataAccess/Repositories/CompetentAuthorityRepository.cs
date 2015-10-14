namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;

    internal class CompetentAuthorityRepository : ICompetentAuthorityRepository
    {
        private readonly IwsContext context;

        public CompetentAuthorityRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<CompetentAuthority>> GetTransitAuthorities(Guid countryId)
        {
            return await
                context.CompetentAuthorities.Where(
                    c => c.Country.Id == countryId && (c.IsTransitAuthority == null || c.IsTransitAuthority == true))
                    .OrderBy(x => x.Code)
                    .ToArrayAsync();
        }

        public async Task<IEnumerable<CompetentAuthority>> GetCompetentAuthorities(Guid countryId)
        {
            return await
                context.CompetentAuthorities.Where(
                    c => c.Country.Id == countryId && (c.IsTransitAuthority == null || c.IsTransitAuthority == false))
                    .OrderBy(x => x.Code)
                    .ToArrayAsync();
        }
    }
}