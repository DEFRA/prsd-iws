namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class ExporterMapping : EntityTypeConfiguration<Exporter>
    {
        public ExporterMapping()
        {
            ToTable("Exporter", "Notification");
        }
    }
}
