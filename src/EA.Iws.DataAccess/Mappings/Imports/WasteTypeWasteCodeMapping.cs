namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification.WasteCodes;

    internal class WasteTypeWasteCodeMapping : EntityTypeConfiguration<WasteTypeWasteCode>
    {
        public WasteTypeWasteCodeMapping()
        {
            ToTable("WasteCode", "ImportNotification");

            Property(x => x.Id).IsRequired();
        }
    }
}