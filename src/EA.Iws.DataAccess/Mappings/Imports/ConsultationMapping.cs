namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment;

    internal class ConsultationMapping : EntityTypeConfiguration<Consultation>
    {
        public ConsultationMapping()
        {
            ToTable("Consultation", "ImportNotification");

            Property(x => x.NotificationId).IsRequired();
            Property(x => x.LocalAreaId).IsOptional();
        }
    }
}