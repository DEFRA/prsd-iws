namespace EA.Iws.DataAccess.Mappings.Exports
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
