namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class ExporterMapping : EntityTypeConfiguration<Exporter>
    {
        public ExporterMapping()
        {
            ToTable("Exporter", "ImportNotification");

            Property(x => x.ImportNotificationId).IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(3000);
        }
    }
}