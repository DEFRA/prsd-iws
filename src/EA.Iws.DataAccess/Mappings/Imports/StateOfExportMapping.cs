namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class StateOfExportMapping : EntityTypeConfiguration<StateOfExport>
    {
        public StateOfExportMapping()
        {
            ToTable("StateOfExport", "ImportNotification");
        }
    }
}