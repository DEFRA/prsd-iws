namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.MovementOperationReceipt;

    internal class MovementOperationReceiptMapping : EntityTypeConfiguration<MovementOperationReceipt>
    {
        public MovementOperationReceiptMapping()
        {
            ToTable("MovementOperationReceipt", "Notification");
        }
    }
}
