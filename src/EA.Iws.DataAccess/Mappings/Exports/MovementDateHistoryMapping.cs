namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;

    internal class MovementDateHistoryMapping : EntityTypeConfiguration<MovementDateHistory>
    {
        public MovementDateHistoryMapping()
        {
            ToTable("MovementDateHistory", "Notification");
        }
    }
}