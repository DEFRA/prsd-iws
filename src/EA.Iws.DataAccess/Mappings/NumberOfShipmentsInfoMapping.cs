namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;
    public class NumberOfShipmentsInfoMapping : EntityTypeConfiguration<NumberOfShipmentsInfo>
    {
        public NumberOfShipmentsInfoMapping()
        {
            ToTable("NumberOfShipmentsInfo", "Business");

            Property(x => x.NumberOfShipments).IsRequired();
            Property(x => x.Quantity).IsRequired();
            Property(x => x.FirstDate).IsRequired();
            Property(x => x.LastDate).IsRequired();
            Property(x => x.Units.Value).IsRequired();
        }
    }
}