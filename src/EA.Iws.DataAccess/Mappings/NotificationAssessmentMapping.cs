namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationAssessment;

    internal class NotificationAssessmentMapping : EntityTypeConfiguration<NotificationAssessment>
    {
        public NotificationAssessmentMapping()
        {
            ToTable("Notification", "NotificationAssessment");
        }
    }
}