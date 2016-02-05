namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Core.ComponentRegistration;

    [AutoRegister]
    public class ReleaseImportFinancialGuarantee
    {
        private readonly IImportFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly IImportFinancialGuaranteeReleaseRepository releaseRepository;

        public ReleaseImportFinancialGuarantee(IImportFinancialGuaranteeRepository financialGuaranteeRepository,
            IImportFinancialGuaranteeReleaseRepository releaseRepository)
        {
            this.financialGuaranteeRepository = financialGuaranteeRepository;
            this.releaseRepository = releaseRepository;
        }

        public async Task<ImportFinancialGuaranteeRelease> Release(DecisionData decision)
        {
            var guarantee = await financialGuaranteeRepository.GetByNotificationId(decision.ImportNotificationId);

            var release = guarantee.Release(decision.Date);

            releaseRepository.Add(release);

            return release;
        } 
    }
}
