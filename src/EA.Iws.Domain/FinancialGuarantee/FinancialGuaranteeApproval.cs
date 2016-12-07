namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Prsd.Core;

    [AutoRegister]
    public class FinancialGuaranteeApproval
    {
        private readonly IFinancialGuaranteeRepository repository;

        public FinancialGuaranteeApproval(IFinancialGuaranteeRepository repository)
        {
            this.repository = repository;
        }

        public async Task Approve(Guid notificationId, Guid financialGuaranteeId, ApprovalData approvalData)
        {
            var financialGuaranteeCollection = await repository.GetByNotificationId(notificationId);
            var approvedFinancialGuarantee = financialGuaranteeCollection.GetCurrentApprovedFinancialGuarantee();
            var financialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(financialGuaranteeId);

            financialGuarantee.Approve(approvalData);

            if (approvedFinancialGuarantee != null)
            {
                if (approvedFinancialGuarantee.ReferenceNumber == financialGuarantee.ReferenceNumber)
                {
                    approvedFinancialGuarantee.Supersede();
                }
                else
                {
                    approvedFinancialGuarantee.Release(SystemTime.UtcNow);
                }
            }
        }
    }
}