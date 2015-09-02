namespace EA.Iws.DataAccess.Mappings
{
    using Domain.Movement;
    using System.Data.Entity.ModelConfiguration;
    using EA.Prsd.Core.Helpers;
 
    internal class MovementCarrierMapping : EntityTypeConfiguration<MovementCarrier>
    {
        public MovementCarrierMapping()
        {
            ToTable("MovementCarrier", "Notification");
        }
    }
}
