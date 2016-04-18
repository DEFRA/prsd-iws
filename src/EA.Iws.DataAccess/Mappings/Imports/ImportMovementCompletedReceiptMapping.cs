namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportMovement;

    internal class ImportMovementCompletedReceiptMapping : EntityTypeConfiguration<ImportMovementCompletedReceipt>
    {
        public ImportMovementCompletedReceiptMapping()
        {
            ToTable("MovementOperationReceipt", "ImportNotification");
        }
    }
}
