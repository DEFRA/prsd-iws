namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication.Recovery;

    internal class RecoveryInfoMapping : EntityTypeConfiguration<RecoveryInfo>
    {
        public RecoveryInfoMapping()
        {
            ToTable("RecoveryInfo", "Notification");
        }
    }
}