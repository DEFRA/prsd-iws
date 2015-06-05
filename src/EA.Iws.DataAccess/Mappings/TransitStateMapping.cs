namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.TransportRoute;

    internal class TransitStateMapping : EntityTypeConfiguration<TransitState>
    {
        public TransitStateMapping()
        {
            this.ToTable("TransitState", "Notification");
        }
    }
}
