namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;

    public class ImportFinancialGuaranteeMapping : EntityTypeConfiguration<ImportFinancialGuarantee>
    {
        public ImportFinancialGuaranteeMapping()
        {
            ToTable("FinancialGuarantee", "ImportNotification");
        }
    }
}
