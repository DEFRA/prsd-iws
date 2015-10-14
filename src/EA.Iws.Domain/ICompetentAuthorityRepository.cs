namespace EA.Iws.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICompetentAuthorityRepository
    {
        Task<IEnumerable<CompetentAuthority>> GetTransitAuthorities(Guid countryId);

        Task<IEnumerable<CompetentAuthority>> GetCompetentAuthorities(Guid countryId);
    }
}
