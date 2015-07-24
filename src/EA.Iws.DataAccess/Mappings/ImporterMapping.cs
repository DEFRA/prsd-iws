namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class ImporterMapping : EntityTypeConfiguration<Importer>
    {
        public ImporterMapping()
        {
            this.ToTable("Importer", "Business");
        }
    }
}
