namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportMovement;

    internal class ImportMovementRejectionMapping : EntityTypeConfiguration<ImportMovementRejection>
    {
        public ImportMovementRejectionMapping()
        {
            ToTable("MovementRejection", "ImportNotification");

            Property(x => x.Reason).HasMaxLength(2048);
        }
    }
}
