namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;

    internal class ImportFinancialGuaranteeApprovalMapping : EntityTypeConfiguration<ImportFinancialGuaranteeApproval>
    {
        public ImportFinancialGuaranteeApprovalMapping()
        {
            ToTable("FinancialGuaranteeApproval", "ImportNotification");

            Property(x => x.ReferenceNumber).HasMaxLength(70);
        }
    }
}
