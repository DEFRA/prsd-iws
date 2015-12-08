namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class WasteCompositionMapping : EntityTypeConfiguration<WasteComposition>
    {
        public WasteCompositionMapping()
        {
            ToTable("WasteComposition", "Notification");

            Property(x => x.Constituent).IsRequired().HasMaxLength(1024);
            Property(x => x.MinConcentration).IsRequired();
            Property(x => x.MaxConcentration).IsRequired();
        }
    }
}
