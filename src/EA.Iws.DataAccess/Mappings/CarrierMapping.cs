namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    internal class CarrierMapping : EntityTypeConfiguration<Carrier>
    {
        public CarrierMapping()
        {
            this.ToTable("Carrier", "Business");
        }
    }
}
