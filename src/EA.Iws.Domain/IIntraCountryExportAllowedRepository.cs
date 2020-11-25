namespace EA.Iws.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IIntraCountryExportAllowedRepository
    {
        Task<IEnumerable<IntraCountryExportAllowed>> GetAll();

        Task<IEnumerable<IntraCountryExportAllowed>> GetImportCompetentAuthorities(Guid exportCompetentAuthorityId);
    }
}