namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;

    internal class ImportFinancialGuaranteeStatusChangeMapping : EntityTypeConfiguration<ImportFinancialGuaranteeStatusChange>
    {
        public ImportFinancialGuaranteeStatusChangeMapping()
        {
            ToTable("FinancialGuaranteeStatusChange", "ImportNotification");
        }
    }
}
