namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Core.ComponentRegistration;

    [AutoRegister]
    public class ApproveImportFinancialGuarantee
    {
        private readonly IImportFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly IImportFinancialGuaranteeApprovalRepository approvalRepository;

        public ApproveImportFinancialGuarantee(IImportFinancialGuaranteeRepository financialGuaranteeRepository,
            IImportFinancialGuaranteeApprovalRepository approvalRepository)
        {
            this.financialGuaranteeRepository = financialGuaranteeRepository;
            this.approvalRepository = approvalRepository;
        }

        public async Task<ImportFinancialGuaranteeApproval> Approve(DecisionData decision, string reference)
        {
            var guarantee = await financialGuaranteeRepository.GetByNotificationId(decision.ImportNotificationId);

            var approval = guarantee.Approve(decision.Date, reference);

            approvalRepository.Add(approval);

            return approval;
        }
    }
}
