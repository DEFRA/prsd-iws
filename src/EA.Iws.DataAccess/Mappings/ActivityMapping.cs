namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class ActivityMapping : EntityTypeConfiguration<Activity>
    {
        public ActivityMapping()
        {
            ToTable("Activity", "Lookup");
        }
    }
}