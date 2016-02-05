namespace EA.Iws.DataAccess.Repositories.Imports
{
    using Domain.ImportNotificationAssessment.FinancialGuarantee;

    internal class ImportFinancialGuaranteeRefusalRepository : IImportFinancialGuaranteeRefusalRepository
    {
        private readonly ImportNotificationContext context;

        public ImportFinancialGuaranteeRefusalRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public void Add(ImportFinancialGuaranteeRefusal refusal)
        {
            context.ImportFinancialGuaranteeRefusals.Add(refusal);
        }
    }
}