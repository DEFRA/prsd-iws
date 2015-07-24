namespace EA.Iws.DataAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class WasteCodeMapping : EntityTypeConfiguration<WasteCode>
    {
        public WasteCodeMapping()
        {
            this.ToTable("WasteCode", "Lookup");

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Code).HasMaxLength(50).IsRequired();
            Property(x => x.Description).IsRequired();
        }
    }
}
