namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.TransportRoute;

    internal class StateOfExportMapping : EntityTypeConfiguration<StateOfExport>
    {
        public StateOfExportMapping()
        {
            this.ToTable("StateOfExport", "Notification");
        }
    }
}
