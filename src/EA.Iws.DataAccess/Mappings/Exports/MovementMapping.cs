namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;
    using Prsd.Core.Helpers;

    public class MovementMapping : EntityTypeConfiguration<Movement>
    {
        public MovementMapping()
        {
            ToTable("Movement", "Notification");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<Movement, ICollection<MovementStatusChange>>(
                    "StatusChangeCollection"))
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

            Property(x => x.StatsMarking).HasMaxLength(1024);
        }
    }
}