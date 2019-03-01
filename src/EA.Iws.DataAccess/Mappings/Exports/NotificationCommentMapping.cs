namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationAssessment;

    internal class NotificationCommentMapping : EntityTypeConfiguration<NotificationComment>
    {
        public NotificationCommentMapping()
        {
            ToTable("Comments", "Notification");

            Property(x => x.Comment).HasMaxLength(500);
        }
    }
}
