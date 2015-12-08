namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationAssessment;

    internal class NotificationDecisionMapping : EntityTypeConfiguration<NotificationDecision>
    {
        public NotificationDecisionMapping()
        {
            ToTable("NotificationDecision", "Notification");
        }
    }
}