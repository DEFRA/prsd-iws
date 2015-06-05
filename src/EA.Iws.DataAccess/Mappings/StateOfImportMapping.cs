namespace EA.Iws.DataAccess.Mappings
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
