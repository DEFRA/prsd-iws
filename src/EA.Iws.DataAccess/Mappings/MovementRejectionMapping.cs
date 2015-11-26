namespace EA.Iws.DataAccess.Mappings
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
