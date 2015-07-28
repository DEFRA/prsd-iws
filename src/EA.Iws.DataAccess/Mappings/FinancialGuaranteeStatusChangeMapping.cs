namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.FinancialGuarantee;

    internal class FinancialGuaranteeStatusChangeMapping : EntityTypeConfiguration<FinancialGuaranteeStatusChange>
    {
        public FinancialGuaranteeStatusChangeMapping()
        {
            ToTable("FinancialGuaranteeStatusChange", "Notification");
        }
    }
}
