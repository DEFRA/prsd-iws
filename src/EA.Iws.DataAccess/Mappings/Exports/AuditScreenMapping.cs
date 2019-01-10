namespace EA.Iws.DataAccess.Mappings.Exports
{
    using Domain.NotificationApplication;
    using System.Data.Entity.ModelConfiguration;

    internal class AuditScreenMapping : EntityTypeConfiguration<AuditScreen>
    {
        public AuditScreenMapping()
        {
            ToTable("Screen", "Lookup");
        }
    }
}
