namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;

    internal class FinancialGuaranteeDecisionRepository : IFinancialGuaranteeDecisionRepository
    {
        private readonly IwsContext context;

        public FinancialGuaranteeDecisionRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<FinancialGuaranteeDecision>> GetByNotificationId(Guid notificationId)
        {
            var financialGuarantee = await context.FinancialGuarantees.SingleOrDefaultAsync(fg => fg.NotificationApplicationId == notificationId);

            var result = new List<FinancialGuaranteeDecision>();
            foreach (var statusChange in financialGuarantee.StatusChanges)
            {
                if (statusChange.Status == FinancialGuaranteeStatus.Approved)
                {
                    result.Add(new FinancialGuaranteeDecision
                    {
                        NotificationId = financialGuarantee.NotificationApplicationId,
                        Date = statusChange.ChangeDate,
                        Status = statusChange.Status,
                        ApprovedFrom = financialGuarantee.ApprovedFrom,
                        ApprovedTo = financialGuarantee.ApprovedTo
                    });
                }
                else if (statusChange.Status == FinancialGuaranteeStatus.Released 
                    || statusChange.Status == FinancialGuaranteeStatus.Refused)
                {
                    result.Add(new FinancialGuaranteeDecision
                    {
                        NotificationId = financialGuarantee.NotificationApplicationId,
                        Date = statusChange.ChangeDate,
                        Status = statusChange.Status
                    });
                }
            }

            return result;
        }
    }
}