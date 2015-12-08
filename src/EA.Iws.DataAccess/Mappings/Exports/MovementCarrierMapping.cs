namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;

    internal class MovementCarrierMapping : EntityTypeConfiguration<MovementCarrier>
    {
        public MovementCarrierMapping()
        {
            ToTable("MovementCarrier", "Notification");
        }
    }
}
