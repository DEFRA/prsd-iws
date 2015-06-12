namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    internal class WasteCodeMapping : EntityTypeConfiguration<WasteCode>
    {
        public WasteCodeMapping()
        {
            this.ToTable("WasteCode", "Lookup");

            Property(x => x.Code).IsRequired();
            Property(x => x.Description).IsRequired();
        }
    }
}
