namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;

    public interface IFinancialGuaranteeRepository
    {
        Task<FinancialGuarantee> GetByNotificationId(Guid notificationId);

        Task<FinancialGuaranteeStatus> GetStatusByNotificationId(Guid notificationId);
    }
}