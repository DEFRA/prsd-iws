namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment;

    internal class ImportNotificationStatusChangeMapping : EntityTypeConfiguration<ImportNotificationStatusChange>
    {
        public ImportNotificationStatusChangeMapping()
        {
            ToTable("NotificationStatusChange", "ImportNotification");
        }
    }
}