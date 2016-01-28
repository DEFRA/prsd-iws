namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment.Decision;

    internal class ImportWithdrawnMapping : EntityTypeConfiguration<ImportWithdrawn>
    {
        public ImportWithdrawnMapping()
        {
            ToTable("Withdrawn", "ImportNotification");

            Property(x => x.Reasons).HasMaxLength(4000);
        }
    }
}
