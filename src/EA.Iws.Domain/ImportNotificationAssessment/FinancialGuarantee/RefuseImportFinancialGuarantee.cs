namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Core.ComponentRegistration;

    [AutoRegister]
    public class RefuseImportFinancialGuarantee
    {
        private readonly IImportFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly IImportFinancialGuaranteeRefusalRepository refusalRepository;

        public RefuseImportFinancialGuarantee(IImportFinancialGuaranteeRepository financialGuaranteeRepository,
            IImportFinancialGuaranteeRefusalRepository refusalRepository)
        {
            this.financialGuaranteeRepository = financialGuaranteeRepository;
            this.refusalRepository = refusalRepository;
        }

        public async Task<ImportFinancialGuaranteeRefusal> Refuse(DecisionData decision, string reason)
        {
            var guarantee = await financialGuaranteeRepository.GetByNotificationId(decision.ImportNotificationId);

            var refusal = guarantee.Refuse(decision.Date, reason);

            refusalRepository.Add(refusal);

            return refusal;
        }
    }
}
