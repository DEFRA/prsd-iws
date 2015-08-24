namespace EA.Iws.DataAccess.Mappings
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using EA.Prsd.Core.Helpers;

    public class MovementMapping : EntityTypeConfiguration<Movement>
    {
        public MovementMapping()
        {
            ToTable("Movement", "Notification");

            Property(x => x.NotificationApplicationId).HasColumnName("NotificationId");

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
        }
    }
}
