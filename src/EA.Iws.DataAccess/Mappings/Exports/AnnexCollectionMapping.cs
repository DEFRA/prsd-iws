namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication.Annexes;

    internal class AnnexCollectionMapping : EntityTypeConfiguration<AnnexCollection>
    {
        public AnnexCollectionMapping()
        {
            ToTable("AnnexCollection", "Notification");
        }
    }
}
