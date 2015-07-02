namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    internal class ShipmentQuantityRangeMapping : EntityTypeConfiguration<ShipmentQuantityRange>
    {
        public ShipmentQuantityRangeMapping()
        {
            ToTable("ShipmentQuantityRange", "Lookup");
        }
    }
}