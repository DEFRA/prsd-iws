namespace EA.Iws.DataAccess.Mappings.Exports
{
    using EA.Iws.Domain.NotificationApplication;
    using System.Data.Entity.ModelConfiguration;

    internal class WasteComponentInfoMapping : EntityTypeConfiguration<WasteComponentInfo>
    {
        public WasteComponentInfoMapping()
        {
            ToTable("WasteComponentInfo", "Notification");
        }
    }
}
