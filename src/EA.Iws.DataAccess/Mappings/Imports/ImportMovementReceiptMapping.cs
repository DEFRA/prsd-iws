namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportMovement;

    internal class ImportMovementReceiptMapping : EntityTypeConfiguration<ImportMovementReceipt>
    {
        public ImportMovementReceiptMapping()
        {
            ToTable("MovementReceipt", "ImportNotification");
        }
    }
}
