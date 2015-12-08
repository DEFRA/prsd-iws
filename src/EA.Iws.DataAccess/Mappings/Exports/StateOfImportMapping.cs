namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.TransportRoute;

    internal class StateOfImportMapping : EntityTypeConfiguration<StateOfImport>
    {
        public StateOfImportMapping()
        {
            this.ToTable("StateOfImport", "Notification");
        }
    }
}
