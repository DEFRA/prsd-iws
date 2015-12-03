namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportMovement;

    public class ImportMovementMapping : EntityTypeConfiguration<ImportMovement>
    {
        public ImportMovementMapping()
        {
            ToTable("ImportMovement", "Notification");
        }
    }
}
