namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class ImportNotificationMapping : EntityTypeConfiguration<ImportNotification>
    {
        public ImportNotificationMapping()
        {
            ToTable("ImportNotification", "Notification");
        }
    }
}
