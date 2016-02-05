namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;

    public interface IImportFinancialGuaranteeApprovalRepository
    {
        void Add(ImportFinancialGuaranteeApproval approval);

        Task<ImportFinancialGuaranteeApproval> GetByNotificationId(Guid notificationId);
    }
}
