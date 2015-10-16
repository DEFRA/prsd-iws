namespace EA.Iws.DataAccess.Mappings
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Helpers;

    public class MovementMapping : EntityTypeConfiguration<Movement>
    {
        public MovementMapping()
        {
            ToTable("Movement", "Notification");

            Property(x => x.Units).HasColumnName("Unit");

            Property(x => x.Quantity).HasPrecision(18, 4);

            HasMany(
                 ExpressionHelper.GetPrivatePropertyExpression<Movement, ICollection<PackagingInfo>>(
                     "PackagingInfosCollection"))
                 .WithMany()
                 .Map(m =>
                 {
                     m.MapLeftKey("MovementId");
                     m.MapRightKey("PackagingInfoId");
                     m.ToTable("MovementPackagingInfo", "Notification");
                 });

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<Movement, ICollection<MovementStatusChange>>(
                    "StatusChangeCollection"))
                .WithRequired()
                .Map(m => m.MapKey("MovementId"));

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<Movement, ICollection<MovementCarrier>>(
                    "MovementCarriersCollection"))
                .WithRequired()
                .Map(m => m.MapKey("MovementId"));

            HasOptional(
                ExpressionHelper.GetPrivatePropertyExpression<Movement, MovementReceipt>("Receipt"))
                .WithRequired()
                .Map(m => m.MapKey("MovementId"));

            HasOptional(
                ExpressionHelper.GetPrivatePropertyExpression<Movement, MovementCompletedReceipt>("CompletedReceipt"))
                .WithRequired()
                .Map(m => m.MapKey("MovementId"));
        }
    }
}
