namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;

    internal class MovementReceiptMapping : EntityTypeConfiguration<MovementReceipt>
    {
        public MovementReceiptMapping()
        {
            ToTable("MovementReceipt", "Notification");

            Property(x => x.QuantityReceived.Quantity).HasColumnName("Quantity").HasPrecision(18, 4);

            Property(x => x.QuantityReceived.Units).HasColumnName("Unit");
        }
    }
}
