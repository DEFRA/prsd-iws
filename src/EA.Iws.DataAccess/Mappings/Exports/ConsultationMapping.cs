namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationAssessment;

    internal class ConsultationMapping : EntityTypeConfiguration<Consultation>
    {
        public ConsultationMapping()
        {
            ToTable("Consultation", "Notification");

            Property(x => x.NotificationId).IsRequired();
            Property(x => x.LocalAreaId).IsOptional();
        }
    }
}