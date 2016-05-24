namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Domain.Security;

    internal class ImportFinancialGuaranteeRepository : IImportFinancialGuaranteeRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportFinancialGuaranteeRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<ImportFinancialGuarantee> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.ImportFinancialGuarantees.SingleAsync(f => f.ImportNotificationId == notificationId);
        }

        public async Task<ImportFinancialGuarantee> GetByNotificationIdOrDefault(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.ImportFinancialGuarantees.SingleOrDefaultAsync(f => f.ImportNotificationId == notificationId);
        }

        public void Add(ImportFinancialGuarantee importFinancialGuarantee)
        {
            context.ImportFinancialGuarantees.Add(importFinancialGuarantee);
        }
    }
}
