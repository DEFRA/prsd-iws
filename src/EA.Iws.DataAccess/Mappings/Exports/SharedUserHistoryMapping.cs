namespace EA.Iws.DataAccess.Mappings.Exports
{
    using Domain.NotificationApplication;
    using System.Data.Entity.ModelConfiguration;

    internal class SharedUserHistoryMapping : EntityTypeConfiguration<SharedUserHistory>
    {
        public SharedUserHistoryMapping()
        {
            ToTable("SharedUserHistory", "Notification");
        }
    }
}
