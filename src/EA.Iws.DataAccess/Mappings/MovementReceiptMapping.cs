namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.MovementReceipt;

    internal class MovementReceiptMapping : EntityTypeConfiguration<MovementReceipt>
    {
        public MovementReceiptMapping()
        {
            ToTable("MovementReceipt", "Notification");
        }
    }
}
