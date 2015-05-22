namespace EA.Iws.DataAccess.Mappings
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;
    using Prsd.Core;

    internal class NotificationApplicationMapping : EntityTypeConfiguration<NotificationApplication>
    {
        public NotificationApplicationMapping()
        {
            ToTable("Notification", "Notification");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<NotificationApplication, ICollection<Producer>>(
                    "ProducersCollection"))
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<NotificationApplication, ICollection<Facility>>(
                    "FacilitiesCollection"))
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            HasOptional(x => x.Exporter)
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            HasOptional(x => x.Importer)
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            Property(x => x.CompetentAuthority.Value)
                .IsRequired();

            Property(x => x.NotificationType.Value)
                .IsRequired();

            Property(x => x.UserId)
                .IsRequired();

            Property(x => x.NotificationNumber)
                .IsRequired()
                .HasMaxLength(50);

            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}