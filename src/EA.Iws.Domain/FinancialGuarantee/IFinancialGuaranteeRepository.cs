namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;

    public interface IFinancialGuaranteeRepository
    {
        Task<FinancialGuarantee> GetByNotificationId(Guid notificationId);
    }
}