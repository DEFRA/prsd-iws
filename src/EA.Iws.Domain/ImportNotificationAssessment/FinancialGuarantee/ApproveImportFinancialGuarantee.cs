namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
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

        public async Task<ImportFinancialGuaranteeApproval> Approve(DecisionData decision, DateRange validDates, int activeLoads, string reference)
        {
            var guarantee = await financialGuaranteeRepository.GetByNotificationId(decision.ImportNotificationId);

            var approval = guarantee.Approve(decision.Date, validDates, activeLoads, reference);

            approvalRepository.Add(approval);

            return approval;
        }

        public async Task<ImportFinancialGuaranteeApproval> ApproveBlanketBond(DecisionData decision, DateTime validFrom, int activeLoads, string bondReference)
        {
            var guarantee = await financialGuaranteeRepository.GetByNotificationId(decision.ImportNotificationId);

            var approval = guarantee.ApproveBlanketBond(decision.Date, validFrom, activeLoads, bondReference);

            approvalRepository.Add(approval);

            return approval;
        }
    }
}
