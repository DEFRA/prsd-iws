namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class ProducerMapping : EntityTypeConfiguration<Producer>
    {
        public ProducerMapping()
        {
            this.ToTable("Producer", "Notification");

            Property(x => x.IsSiteOfExport).IsRequired();
        }
    }
}
