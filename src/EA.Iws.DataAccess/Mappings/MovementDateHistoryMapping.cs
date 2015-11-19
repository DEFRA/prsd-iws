namespace EA.Iws.DataAccess.Mappings
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