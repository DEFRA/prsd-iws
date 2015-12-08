namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class WasteAdditionalInformationMapping : EntityTypeConfiguration<WasteAdditionalInformation>
    {
        public WasteAdditionalInformationMapping()
        {
            ToTable("WasteAdditionalInformation", "Notification");
            Property(x => x.Constituent).IsRequired().HasMaxLength(1024);
            Property(x => x.MinConcentration).IsRequired();
            Property(x => x.MaxConcentration).IsRequired();
        }
    }
}