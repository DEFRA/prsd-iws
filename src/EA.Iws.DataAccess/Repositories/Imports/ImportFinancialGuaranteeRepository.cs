namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;

    internal class ImportFinancialGuaranteeRepository : IImportFinancialGuaranteeRepository
    {
        private readonly ImportNotificationContext context;

        public ImportFinancialGuaranteeRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<ImportFinancialGuarantee> GetByNotificationId(Guid notificationId)
        {
            return await context.ImportFinancialGuarantees.SingleAsync(f => f.ImportNotificationId == notificationId);
        }

        public async Task<ImportFinancialGuarantee> GetByNotificationIdOrDefault(Guid notificationId)
        {
            return await context.ImportFinancialGuarantees.SingleOrDefaultAsync(f => f.ImportNotificationId == notificationId);
        }

        public void Add(ImportFinancialGuarantee importFinancialGuarantee)
        {
            context.ImportFinancialGuarantees.Add(importFinancialGuarantee);
        }
    }
}
