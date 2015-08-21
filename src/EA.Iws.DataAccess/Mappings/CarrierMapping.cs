namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class CarrierMapping : EntityTypeConfiguration<Carrier>
    {
        public CarrierMapping()
        {
            this.ToTable("Carrier", "Notification");
        }
    }
}
