﻿namespace EA.Iws.Domain
{
    using EA.Iws.Core.Notification;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IIntraCountryExportAllowedRepository
    {
        Task<IEnumerable<IntraCountryExportAllowed>> GetAllAsync();

        Task<IEnumerable<IntraCountryExportAllowed>> GetImportCompetentAuthorities(UKCompetentAuthority exportCompetentAuthorityId);
    }
}