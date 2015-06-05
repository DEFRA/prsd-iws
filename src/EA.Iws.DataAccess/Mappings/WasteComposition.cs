namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    internal class WasteCompositionMapping : EntityTypeConfiguration<WasteComposition>
    {
        public WasteCompositionMapping()
        {
            ToTable("WasteComposition", "Business");

            Property(x => x.Constituent).IsRequired().HasMaxLength(1024);
            Property(x => x.MinConcentration).IsRequired();
            Property(x => x.MaxConcentration).IsRequired();
        }
    }
}
