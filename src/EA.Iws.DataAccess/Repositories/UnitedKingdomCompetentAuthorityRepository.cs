namespace EA.Iws.DataAccess.Repositories
{
    using EA.Iws.Domain;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using EA.Iws.Core.Notification;

    public class UnitedKingdomCompetentAuthorityRepository : StaticDataCachingRepositoryBase<UnitedKingdomCompetentAuthority>, IUnitedKingdomCompetentAuthorityRepository
    {
        public UnitedKingdomCompetentAuthorityRepository(IwsContext context) : base(context)
        {
        }

        protected override UnitedKingdomCompetentAuthority[] GetFromContext()
        {
            return this.Context.UnitedKingdomCompetentAuthorities.ToArray();
        }

        public async Task<bool> IsCountryUk(Guid countryId)
        {
            return (await GetAllAsync()).Any(ukca => ukca.CompetentAuthority.Country.Id == countryId);
        }

        public async Task<UnitedKingdomCompetentAuthority> GetByCompetentAuthority(UKCompetentAuthority competentAuthority)
        {
            return (await GetAllAsync()).Single(ca => ca.Id == (int)competentAuthority);
        }
    }
}
