namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class ShipmentPeriodMapping : ComplexTypeConfiguration<ShipmentPeriod>
    {
        public ShipmentPeriodMapping()
        {
            Property(x => x.FirstDate).HasColumnName("FirstDate").IsRequired();
            Property(x => x.LastDate).HasColumnName("LastDate").IsRequired();
        }
    }
}
