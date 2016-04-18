namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;

    internal class ImportFinancialGuaranteeApprovalRepository : IImportFinancialGuaranteeApprovalRepository
    {
        private readonly ImportNotificationContext context;

        public ImportFinancialGuaranteeApprovalRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public void Add(ImportFinancialGuaranteeApproval approval)
        {
            context.ImportFinancialGuaranteeApprovals.Add(approval);
        }

        public async Task<ImportFinancialGuaranteeApproval> GetByNotificationId(Guid notificationId)
        {
            return await context.ImportFinancialGuaranteeApprovals.SingleAsync(a => a.ImportNotificationId == notificationId);
        }
    }
}
