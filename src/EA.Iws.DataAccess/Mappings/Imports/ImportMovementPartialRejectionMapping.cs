namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportMovement;

    internal class ImportMovementPartialRejectionMapping : EntityTypeConfiguration<ImportMovementPartialRejection>
    {
        public ImportMovementPartialRejectionMapping()
        {
            ToTable("MovementPartialRejection", "ImportNotification");

            Property(x => x.Reason).HasMaxLength(2048);
        }
    }
}
