namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class UserHistoryMapping : EntityTypeConfiguration<UserHistory>
    {
        public UserHistoryMapping()
        {
            ToTable("UserHistory", "Notification");
        }
    }
}
