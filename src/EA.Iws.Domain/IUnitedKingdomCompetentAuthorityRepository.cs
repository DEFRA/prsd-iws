namespace EA.Iws.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EA.Iws.Core.Notification;

    public interface IUnitedKingdomCompetentAuthorityRepository
    {
        Task<IEnumerable<UnitedKingdomCompetentAuthority>> GetAllAsync();
        IEnumerable<UnitedKingdomCompetentAuthority> GetAll();
        Task<UnitedKingdomCompetentAuthority> GetByCompetentAuthority(UKCompetentAuthority competentAuthority);

        Task<bool> IsCountryUk(Guid countryId);
    }
}
