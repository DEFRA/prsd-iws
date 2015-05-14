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

            HasMany(ExpressionHelper.GetPrivatePropertyExpression<NotificationApplication, ICollection<Producer>>("ProducersCollection")).WithMany().Map(m =>
            {
                m.MapLeftKey("NotificationId");
                m.MapRightKey("ProducerId");
                m.ToTable("NotificationProducer",
                    "Business");
            });

            Property(x => x.CompetentAuthority.Value)
                .IsRequired();

            Property(x => x.WasteAction.Value)
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