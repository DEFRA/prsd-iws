namespace EA.Iws.DataAccess.Repositories.Imports
{
    using Domain.ImportNotificationAssessment.FinancialGuarantee;

    internal class ImportFinancialGuaranteeReleaseRepository : IImportFinancialGuaranteeReleaseRepository
    {
        private readonly ImportNotificationContext context;

        public ImportFinancialGuaranteeReleaseRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public void Add(ImportFinancialGuaranteeRelease release)
        {
            context.ImportFinancialGuaranteeReleases.Add(release);
        }
    }
}