namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;

    internal class MovementStatusChangeMapping : EntityTypeConfiguration<MovementStatusChange>
    {
        public MovementStatusChangeMapping()
        {
            ToTable("MovementStatusChange", "Notification");
        }
    }
}
