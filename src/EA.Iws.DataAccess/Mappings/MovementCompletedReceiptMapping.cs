namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;

    internal class MovementCompletedReceiptMapping : EntityTypeConfiguration<MovementCompletedReceipt>
    {
        public MovementCompletedReceiptMapping()
        {
            ToTable("MovementOperationReceipt", "Notification");
        }
    }
}
