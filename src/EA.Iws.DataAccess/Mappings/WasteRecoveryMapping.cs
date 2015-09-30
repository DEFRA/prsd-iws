namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication.WasteRecovery;

    internal class WasteRecoveryMapping : EntityTypeConfiguration<WasteRecovery>
    {
        public WasteRecoveryMapping()
        {
            ToTable("RecoveryInfo", "Notification");

            Property(x => x.PercentageRecoverable.Value).HasColumnName("PercentageRecoverable");
        }
    }
}