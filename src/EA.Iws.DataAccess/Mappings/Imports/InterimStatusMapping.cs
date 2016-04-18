namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class InterimStatusMapping : EntityTypeConfiguration<InterimStatus>
    {
        public InterimStatusMapping()
        {
            ToTable("InterimStatus", "ImportNotification");

            Property(x => x.ImportNotificationId).IsRequired();
        }
    }
}