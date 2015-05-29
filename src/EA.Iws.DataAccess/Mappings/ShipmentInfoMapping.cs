namespace EA.Iws.DataAccess.Mappings
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;
    using Prsd.Core;

    public class ShipmentInfoMapping : EntityTypeConfiguration<ShipmentInfo>
    {
        public ShipmentInfoMapping()
        {
            ToTable("ShipmentInfo", "Business");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<ShipmentInfo, ICollection<PackagingInfo>>(
                    "PackagingInfosCollection"))
                .WithRequired()
                .Map(m => m.MapKey("ShipmentInfoId"));

            Property(x => x.NumberOfShipments).IsRequired();
            Property(x => x.Quantity).IsRequired();
            Property(x => x.FirstDate).IsRequired();
            Property(x => x.LastDate).IsRequired();
            Property(x => x.Units.Value).IsRequired();
            Property(x => x.SpecialHandlingDetails).HasMaxLength(2048);
        }
    }
}