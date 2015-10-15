namespace EA.Iws.DataAccess.Mappings
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
