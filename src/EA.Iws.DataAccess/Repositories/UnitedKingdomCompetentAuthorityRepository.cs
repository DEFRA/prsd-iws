namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using EA.Iws.Core.Notification;
    using EA.Iws.Domain;

    public class UnitedKingdomCompetentAuthorityRepository : IUnitedKingdomCompetentAuthorityRepository
    {
        private readonly IwsContext context;

        public UnitedKingdomCompetentAuthorityRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> IsCountryUk(Guid countryId)
        {
            return (await GetAllAsync()).Any(ukca => ukca.CompetentAuthority.Country.Id == countryId);
        }

        public async Task<UnitedKingdomCompetentAuthority> GetByCompetentAuthority(UKCompetentAuthority competentAuthority)
        {
            return (await GetAllAsync()).Single(ca => ca.Id == (int)competentAuthority);
        }

        public async Task<IEnumerable<UnitedKingdomCompetentAuthority>> GetAllAsync()
        {
            return await this.context.UnitedKingdomCompetentAuthorities.ToArrayAsync();
        }

        public IEnumerable<UnitedKingdomCompetentAuthority> GetAll()
        {
            return this.context.UnitedKingdomCompetentAuthorities;
        }
    }
}
