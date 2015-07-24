namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class RecoveryInfoMapping : EntityTypeConfiguration<RecoveryInfo>
    {
        public RecoveryInfoMapping()
        {
            ToTable("RecoveryInfo", "Business");

            Property(x => x.EstimatedUnit).IsRequired();
            Property(x => x.EstimatedAmount).IsRequired();
            Property(x => x.CostUnit).IsRequired();
            Property(x => x.CostAmount).IsRequired();
            Property(x => x.DisposalUnit).IsOptional();
            Property(x => x.DisposalAmount).IsOptional();
        }
    }
}