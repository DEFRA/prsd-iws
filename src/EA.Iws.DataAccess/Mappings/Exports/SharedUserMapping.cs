namespace EA.Iws.DataAccess.Mappings.Exports
{
    using Domain.NotificationApplication;
    using System.Data.Entity.ModelConfiguration;

    internal class SharedUserMapping : EntityTypeConfiguration<SharedUser>
    {
        public SharedUserMapping()
        {
            ToTable("SharedUser", "Notification");
        }
    }
}
