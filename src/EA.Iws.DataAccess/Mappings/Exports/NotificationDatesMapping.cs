namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationAssessment;

    internal class NotificationDatesMapping : EntityTypeConfiguration<NotificationDates>
    {
        public NotificationDatesMapping()
        {
            ToTable("NotificationDates", "Notification");
        }
    }
}