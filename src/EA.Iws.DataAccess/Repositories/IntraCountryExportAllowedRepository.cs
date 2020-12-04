namespace EA.Iws.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using EA.Iws.Core.Notification;

    internal class IntraCountryExportAllowedRepository : StaticDataCachingRepositoryBase<IntraCountryExportAllowed>, IIntraCountryExportAllowedRepository
    {
        public IntraCountryExportAllowedRepository(IwsContext context) : base(context)
        {
        }

        public async Task<IEnumerable<IntraCountryExportAllowed>> GetImportCompetentAuthorities(UKCompetentAuthority uksCompetentAuthority)
        {
            return (await RetreiveFromCache()).Where(c => (c.ExportCompetentAuthority == uksCompetentAuthority));
        }

        protected override IntraCountryExportAllowed[] GetFromContext()
        {
            return this.Context.IntraCountryExportAllowed.ToArray();
        }
    }
}
