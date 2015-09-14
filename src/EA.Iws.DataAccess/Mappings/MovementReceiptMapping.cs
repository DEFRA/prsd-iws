namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;
    using Domain.MovementReceipt;
    using EA.Prsd.Core.Helpers;

    internal class MovementReceiptMapping : EntityTypeConfiguration<MovementReceipt>
    {
        public MovementReceiptMapping()
        {
            ToTable("MovementReceipt", "Notification");
        }
    }
}
