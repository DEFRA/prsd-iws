namespace EA.Iws.DataAccess.Mappings.Exports
{
    using Core.Notification.Audit;
    using System.Data.Entity.ModelConfiguration;

    internal class AuditMapping : EntityTypeConfiguration<NotificationAudit>
    {
        public AuditMapping()
        {
            ToTable("Audit", "Notification");
        }
    }
}
