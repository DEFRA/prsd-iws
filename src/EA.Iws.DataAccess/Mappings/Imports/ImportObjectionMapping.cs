namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment.Decision;

    internal class ImportObjectionMapping : EntityTypeConfiguration<ImportObjection>
    {
        public ImportObjectionMapping()
        {
            ToTable("Objection", "ImportNotification");

            Property(x => x.Reasons).HasMaxLength(4000);
        }
    }
}
