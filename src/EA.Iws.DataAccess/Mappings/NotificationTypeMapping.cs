namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class NotificationTypeMapping : ComplexTypeConfiguration<NotificationType>
    {
        public NotificationTypeMapping()
        {
            Ignore(x => x.DisplayName);
            Property(x => x.Value)
                .HasColumnName("NotificationType");
        }
    }
}