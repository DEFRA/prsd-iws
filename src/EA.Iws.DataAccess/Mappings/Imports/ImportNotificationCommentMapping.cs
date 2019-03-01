namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment;

    public class ImportNotificationCommentMapping : EntityTypeConfiguration<ImportNotificationComment>
    {
        public ImportNotificationCommentMapping()
        {
            ToTable("Comments", "ImportNotification");

            Property(x => x.Comment).HasMaxLength(500);
        }
    }
}
