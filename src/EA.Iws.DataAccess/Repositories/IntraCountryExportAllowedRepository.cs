namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using EA.Iws.Core.Notification;

    internal class IntraCountryExportAllowedRepository : IIntraCountryExportAllowedRepository
    {
        private readonly IwsContext context;

        public IntraCountryExportAllowedRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<IntraCountryExportAllowed>> GetAll()
        {
            return await
                context.IntraCountryExportAllowed.ToArrayAsync();
        }

        //public async Task<IEnumerable<IntraCountryExportAllowed>> GetImportCompetentAuthorities(Guid exportCompetentAuthority)
        //{
        //    return await
        //        context.IntraCountryExportAllowed.Where(
        //            c => (c.ExportCompetentAuthorityId == exportCompetentAuthority))
        //            .ToArrayAsync();
        //}

        public async Task<IEnumerable<IntraCountryExportAllowed>> GetImportCompetentAuthorities(UKCompetentAuthority uksCompetentAuthority)
        {
            return await
                context.IntraCountryExportAllowed.Where(
                    c => (c.ExportCompetentAuthority == uksCompetentAuthority))
                    .ToArrayAsync();
        }
    }
}