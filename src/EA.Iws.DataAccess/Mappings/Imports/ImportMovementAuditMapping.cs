namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportMovement;

    public class ImportMovementAuditMapping : EntityTypeConfiguration<ImportMovementAudit>
    {
        public ImportMovementAuditMapping()
        {
            ToTable("MovementAudit", "ImportNotification");
        }
    }
}
