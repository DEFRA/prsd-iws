namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class ImporterMapping : EntityTypeConfiguration<Importer>
    {
        public ImporterMapping()
        {
            ToTable("Importer", "ImportNotification");

            Property(x => x.ImportNotificationId).IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(3000);
            Property(x => x.Type).HasColumnName("Type").IsRequired();
            Property(x => x.RegistrationNumber).HasColumnName("RegistrationNumber").IsRequired().HasMaxLength(100);
        }
    }
}