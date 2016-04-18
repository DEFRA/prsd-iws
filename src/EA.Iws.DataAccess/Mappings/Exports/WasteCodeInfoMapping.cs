namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class WasteCodeInfoMapping : EntityTypeConfiguration<WasteCodeInfo>
    {
        public WasteCodeInfoMapping()
        {
            this.ToTable("WasteCodeInfo", "Notification");
        }
    }
}
