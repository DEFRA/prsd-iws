using EA.Iws.Domain.NotificationApplication;
using System.Data.Entity.ModelConfiguration;

namespace EA.Iws.DataAccess.Mappings.Exports
{
    internal class SharedUserMapping : EntityTypeConfiguration<SharedUser>
    {
        public SharedUserMapping()
        {
            ToTable("SharedUser", "Notification");
        }
    }
}
