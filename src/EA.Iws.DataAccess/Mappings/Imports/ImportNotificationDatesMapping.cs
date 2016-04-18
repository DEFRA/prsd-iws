namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment;

    internal class ImportNotificationDatesMapping : EntityTypeConfiguration<ImportNotificationDates>
    {
        public ImportNotificationDatesMapping()
        {
            ToTable("NotificationDates", "ImportNotification");
        }
    }
}