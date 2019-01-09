namespace EA.Iws.DataAccess.Mappings.Exports
{
    using Core.Notification.Audit;
    using System.Data.Entity.ModelConfiguration;

    public class AuditScreenMapping : EntityTypeConfiguration<NotificationAuditScreen>
    {
        public AuditScreenMapping()
        {
            ToTable("Screen", "Lookup");
        }
    }
}
