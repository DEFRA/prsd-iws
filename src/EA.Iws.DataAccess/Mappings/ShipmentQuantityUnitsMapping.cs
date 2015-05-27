namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class ShipmentQuantityUnitsMapping : ComplexTypeConfiguration<ShipmentQuantityUnits>
    {
        public ShipmentQuantityUnitsMapping()
        {
            Ignore(x => x.DisplayName);
            Property(x => x.Value)
                .HasColumnName("Units");
        }
    }
}