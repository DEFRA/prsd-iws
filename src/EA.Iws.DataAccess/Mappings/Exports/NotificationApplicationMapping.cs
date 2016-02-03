namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;
    using Prsd.Core.Helpers;

    internal class NotificationApplicationMapping : EntityTypeConfiguration<NotificationApplication>
    {
        public NotificationApplicationMapping()
        {
            ToTable("Notification", "Notification");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<NotificationApplication, ICollection<OperationInfo>>(
                    "OperationInfosCollection"))
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<NotificationApplication, ICollection<PackagingInfo>>(
                    "PackagingInfosCollection"))
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            HasOptional(x => x.WasteType)
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            HasOptional(x => x.TechnologyEmployed)
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            Property(x => x.CompetentAuthority.Value)
                .IsRequired();

            Property(x => x.NotificationType)
                .IsRequired();

            Property(x => x.UserId)
                .IsRequired();

            Property(x => x.NotificationNumber)
                .IsRequired()
                .HasMaxLength(50);

            Property(x => x.ReasonForExport)
            .HasMaxLength(70);

            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(x => x.SpecialHandlingDetails).HasMaxLength(2048);
            
            Property(x => x.WasteRecoveryInformationProvidedByImporter).HasColumnName("IsRecoveryPercentageDataProvidedByImporter");
            
            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<NotificationApplication, ICollection<PhysicalCharacteristicsInfo>>(
                    "PhysicalCharacteristicsCollection"))
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<NotificationApplication, ICollection<WasteCodeInfo>>(
                    "WasteCodeInfoCollection"))
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));
        }
    }
}