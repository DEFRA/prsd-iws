namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    internal class ProducerMapping : EntityTypeConfiguration<Producer>
    {
        public ProducerMapping()
        {
            this.ToTable("Producer", "Business");

            Property(x => x.IsSiteOfExport).IsRequired();
        }
    }
}
