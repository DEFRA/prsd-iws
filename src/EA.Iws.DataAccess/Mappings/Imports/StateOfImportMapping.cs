namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class StateOfImportMapping : EntityTypeConfiguration<StateOfImport>
    {
        public StateOfImportMapping()
        {
            ToTable("StateOfImport", "ImportNotification");
        }
    }
}