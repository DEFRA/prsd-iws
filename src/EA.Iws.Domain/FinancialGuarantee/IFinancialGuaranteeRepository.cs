namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;

    public interface IFinancialGuaranteeRepository
    {
        Task<FinancialGuaranteeCollection> GetByNotificationId(Guid notificationId);

        Task SetCurrentFinancialGuaranteeDates(Guid notificationId, DateTime? receivedDate, 
            DateTime? completedDate, DateTime? decisionDate);
    }
}