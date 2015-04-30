namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    internal class NotificationApplicationMapping : EntityTypeConfiguration<NotificationApplication>
    {
        public NotificationApplicationMapping()
        {
            ToTable("Notification", "Notification");

            Property(x => x.CompetentAuthority.Value)
                //.HasColumnName("CompetentAuthority")
                .IsRequired();

            Property(x => x.WasteAction.Value)
                //.HasColumnName("WasteAction")
                .IsRequired();

            Property(x => x.UserId)
                .IsRequired();

            Property(x => x.NotificationNumber)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}