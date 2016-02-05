namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;

    internal class ImportFinancialGuaranteeRefusalMapping : EntityTypeConfiguration<ImportFinancialGuaranteeRefusal>
    {
        public ImportFinancialGuaranteeRefusalMapping()
        {
            ToTable("FinancialGuaranteeRefusal", "ImportNotification");

            Property(x => x.Reason).HasMaxLength(2048);
        }
    }
}