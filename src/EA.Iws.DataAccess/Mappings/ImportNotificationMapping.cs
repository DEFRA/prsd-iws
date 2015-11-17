namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class ImportNotificationMapping : EntityTypeConfiguration<ImportNotification>
    {
        public ImportNotificationMapping()
        {
            ToTable("ImportNotification", "Notification");

            Property(x => x.NotificationNumber)
                .IsRequired()
                .HasMaxLength(50);

            Property(x => x.NotificationType)
                .IsRequired();

            Property(x => x.CompetentAuthority.Value)
                .IsRequired();
        }
    }
}
