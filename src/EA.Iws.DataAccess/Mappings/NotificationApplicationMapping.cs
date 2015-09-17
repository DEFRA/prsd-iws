namespace EA.Iws.DataAccess.Mappings
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;
    using Domain.TransportRoute;
    using Prsd.Core.Helpers;

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

            HasOptional(x => x.Exporter)
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            HasOptional(x => x.WasteType)
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            HasOptional(x => x.Importer)
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            HasOptional(x => x.TechnologyEmployed)
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            HasOptional(x => x.RecoveryInfo)
                .WithRequired()
                .Map(m => m.MapKey("NotificationId"));

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<NotificationApplication, ICollection<Carrier>>(
                    "CarriersCollection"))
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

            Property(x => x.ReasonForExport)
            .HasMaxLength(70);

            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(x => x.SpecialHandlingDetails).HasMaxLength(2048);

            Ignore(x => x.MeansOfTransport);
            
            Property(x => x.IsProvidedByImporter).HasColumnName("IsRecoveryPercentageDataProvidedByImporter");

            Property(ExpressionHelper
                .GetPrivatePropertyExpression<NotificationApplication, string>("MeansOfTransportInternal"))
                .HasColumnName("MeansOfTransport");

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