namespace EA.Iws.DataAccess.Mappings.Common
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Finance;
    using Domain.NotificationApplication;

    internal class ActivityMapping : EntityTypeConfiguration<Activity>
    {
        public ActivityMapping()
        {
            ToTable("Activity", "Lookup");
        }
    }
}