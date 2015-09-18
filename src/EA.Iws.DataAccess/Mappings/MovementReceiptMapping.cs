namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.MovementOperationReceipt;
    using Domain.MovementReceipt;
    using Prsd.Core.Helpers;

    internal class MovementReceiptMapping : EntityTypeConfiguration<MovementReceipt>
    {
        public MovementReceiptMapping()
        {
            ToTable("MovementReceipt", "Notification");

            HasOptional(
                ExpressionHelper.GetPrivatePropertyExpression<MovementReceipt, MovementOperationReceipt>("OperationReceipt"))
                .WithRequired()
                .Map(m => m.MapKey("MovementReceiptId"));
        }
    }
}
