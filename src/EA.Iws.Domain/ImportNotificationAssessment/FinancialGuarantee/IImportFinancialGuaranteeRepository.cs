namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;

    public interface IImportFinancialGuaranteeRepository
    {
        Task<ImportFinancialGuarantee> GetByNotificationId(Guid notificationId);

        Task<ImportFinancialGuarantee> GetByNotificationIdOrDefault(Guid notificationId);

        void Add(ImportFinancialGuarantee importFinancialGuarantee);
    }
}
