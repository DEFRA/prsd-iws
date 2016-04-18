namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;

    internal class MovementRejectionMapping : EntityTypeConfiguration<MovementRejection>
    {
        public MovementRejectionMapping()
        {
            ToTable("MovementRejection", "Notification");
        }
    }
}
