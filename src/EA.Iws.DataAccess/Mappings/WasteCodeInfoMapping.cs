namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    internal class WasteCodeInfoMapping : EntityTypeConfiguration<WasteCodeInfo>
    {
        public WasteCodeInfoMapping()
        {
            this.ToTable("WasteCodeInfo", "Business");
        }
    }
}
