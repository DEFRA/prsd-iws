using EA.Iws.Domain.NotificationApplication;
using System.Data.Entity.ModelConfiguration;

namespace EA.Iws.DataAccess.Mappings.Exports
{
    internal class SharedUserHistoryMapping : EntityTypeConfiguration<SharedUserHistory>
    {
        public SharedUserHistoryMapping()
        {
            ToTable("SharedUserHistory", "Notification");
        }
    }
}
