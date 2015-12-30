namespace EA.Iws.DataAccess.Mappings.Common
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Finance;

    internal class ShipmentQuantityRangeMapping : EntityTypeConfiguration<ShipmentQuantityRange>
    {
        public ShipmentQuantityRangeMapping()
        {
            ToTable("ShipmentQuantityRange", "Lookup");
        }
    }
}