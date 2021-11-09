namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;

    internal class MovementPartialRejectionMapping : EntityTypeConfiguration<MovementPartialRejection>
    {
        public MovementPartialRejectionMapping()
        {
            ToTable("MovementPartialRejection", "Notification");
        }
    }
}
