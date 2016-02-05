namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;

    internal class ImportFinancialGuaranteeReleaseMapping : EntityTypeConfiguration<ImportFinancialGuaranteeRelease>
    {
        public ImportFinancialGuaranteeReleaseMapping()
        {
            ToTable("FinancialGuaranteeRelease", "ImportNotification");
        }
    }
}