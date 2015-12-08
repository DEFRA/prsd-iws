namespace EA.Iws.DataAccess.Mappings.Common
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class ShipmentQuantityRangeMapping : EntityTypeConfiguration<ShipmentQuantityRange>
    {
        public ShipmentQuantityRangeMapping()
        {
            ToTable("ShipmentQuantityRange", "Lookup");
        }
    }
}