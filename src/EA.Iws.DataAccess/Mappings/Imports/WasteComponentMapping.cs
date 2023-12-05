namespace EA.Iws.DataAccess.Mappings.Imports
{
    using EA.Iws.Domain.ImportNotification;
    using System.Data.Entity.ModelConfiguration;

    internal class WasteComponentMapping : EntityTypeConfiguration<WasteComponent>
    {
        public WasteComponentMapping()
        {
            ToTable("WasteComponent", "ImportNotification");
        }
    }
}
