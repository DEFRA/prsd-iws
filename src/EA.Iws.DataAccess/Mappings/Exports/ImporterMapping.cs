namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication.Importer;

    internal class ImporterMapping : EntityTypeConfiguration<Importer>
    {
        public ImporterMapping()
        {
            ToTable("Importer", "Notification");
        }
    }
}
