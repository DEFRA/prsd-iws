namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Helpers;

    public class MovementDetailsMapping : EntityTypeConfiguration<MovementDetails>
    {
        public MovementDetailsMapping()
        {
            ToTable("MovementDetails", "Notification");

            Property(x => x.ActualQuantity.Units).HasColumnName("Unit");

            Property(x => x.ActualQuantity.Quantity).HasColumnName("Quantity").HasPrecision(18, 4);

            HasMany(
                 ExpressionHelper.GetPrivatePropertyExpression<MovementDetails, ICollection<PackagingInfo>>(
                     "PackagingInfosCollection"))
                 .WithMany()
                 .Map(m =>
                 {
                     m.MapLeftKey("MovementDetailsId");
                     m.MapRightKey("PackagingInfoId");
                     m.ToTable("MovementPackagingInfo", "Notification");
                 });
        }
    }
}