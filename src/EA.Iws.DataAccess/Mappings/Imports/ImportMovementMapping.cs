namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportMovement;

    public class ImportMovementMapping : EntityTypeConfiguration<ImportMovement>
    {
        public ImportMovementMapping()
        {
            ToTable("Movement", "ImportNotification");
        }
    }
}
