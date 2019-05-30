namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;

    internal class MovementAuditMapping : EntityTypeConfiguration<MovementAudit>
    {
        public MovementAuditMapping()
        {
            ToTable("MovementAudit", "Notification");
        }
    }
}
