namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    public class ShipmentInfoMapping : EntityTypeConfiguration<ShipmentInfo>
    {
        public ShipmentInfoMapping()
        {
            this.ToTable("ShipmentInfo", "Business");

            HasMany(x => x.PackagingTypes)
                .WithRequired()
                .Map(m => m.MapKey("ShippingInfoId"));

            Property(x => x.NumberOfShipments).IsRequired();
            Property(x => x.Quantity).IsRequired();
            Property(x => x.FirstDate).IsRequired();
            Property(x => x.LastDate).IsRequired();
            Property(x => x.Units.Value).IsRequired();
            Property(x => x.SpecialHandlingDetails).HasMaxLength(2048);
        }
    }
}
