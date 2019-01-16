namespace EA.Iws.DataAccess.Mappings.Exports
{
    using Domain.NotificationApplication;
    using System.Data.Entity.ModelConfiguration;

    internal class AuditMapping : EntityTypeConfiguration<Audit>
    {
        public AuditMapping()
        {
            ToTable("Audit", "Notification");
        }
    }
}
