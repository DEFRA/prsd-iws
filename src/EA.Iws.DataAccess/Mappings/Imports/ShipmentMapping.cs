namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class ShipmentMapping : EntityTypeConfiguration<Shipment>
    {
        public ShipmentMapping()
        {
            ToTable("Shipment", "ImportNotification");

            Property(x => x.NumberOfShipments).IsRequired();
            Property(x => x.Period.FirstDate).HasColumnName("FirstDate").IsRequired();
            Property(x => x.Period.LastDate).HasColumnName("LastDate").IsRequired();
            Property(x => x.Quantity.Quantity).HasColumnName("Quantity").IsRequired().HasPrecision(18, 4);
            Property(x => x.Quantity.Units).HasColumnName("Units").IsRequired();
            Property(x => x.ImportNotificationId).IsRequired();
        }
    }
}