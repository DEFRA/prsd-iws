namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Domain.Security;

    internal class ImportFinancialGuaranteeApprovalRepository : IImportFinancialGuaranteeApprovalRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportFinancialGuaranteeApprovalRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public void Add(ImportFinancialGuaranteeApproval approval)
        {
            context.ImportFinancialGuaranteeApprovals.Add(approval);
        }

        public async Task<ImportFinancialGuaranteeApproval> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.ImportFinancialGuaranteeApprovals.SingleAsync(a => a.ImportNotificationId == notificationId);
        }
    }
}
