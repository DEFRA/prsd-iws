namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFinancialGuaranteeDecisionRepository
    {
        Task<IEnumerable<FinancialGuaranteeDecision>> GetByNotificationId(Guid notificationId);
    }
}