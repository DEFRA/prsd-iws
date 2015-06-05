namespace EA.Iws.DataAccess.Mappings
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;
    using Prsd.Core;
    using Prsd.Core.Helpers;

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

            Property(x => x.SpecialHandlingDetails).HasMaxLength(2048);
        }
    }
}