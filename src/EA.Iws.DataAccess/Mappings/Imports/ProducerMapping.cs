namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class ProducerMapping : EntityTypeConfiguration<Producer>
    {
        public ProducerMapping()
        {
            ToTable("Producer", "ImportNotification");

            Property(x => x.ImportNotificationId).IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(3000);
        }
    }
}