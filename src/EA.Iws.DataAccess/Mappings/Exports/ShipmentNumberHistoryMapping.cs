namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication.Shipment;

    public class ShipmentNumberHistoryMapping : EntityTypeConfiguration<ShipmentNumberHistory>
    {
        public ShipmentNumberHistoryMapping()
        {
            ToTable("ShipmentNumberHistory", "Notification");

            Property(x => x.NumberOfShipments).IsRequired();
            Property(x => x.NotificationId).IsRequired();
            Property(x => x.DateChanged).IsRequired();
        }
    }
}
