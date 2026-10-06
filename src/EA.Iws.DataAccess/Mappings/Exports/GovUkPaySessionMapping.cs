namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationAssessment;

    internal class GovUkPaySessionMapping : EntityTypeConfiguration<GovUkPaySession>
    {
        public GovUkPaySessionMapping()
        {
            ToTable("GovUkPaySession", "Notification");

            Property(x => x.PaymentId).IsRequired().HasMaxLength(100);
            Property(x => x.PaymentReference).IsRequired().HasMaxLength(100);
            Property(x => x.SecureToken).IsRequired().HasMaxLength(100);
            Property(x => x.Status).IsRequired().HasMaxLength(50);
            Property(x => x.Amount).HasPrecision(12, 2);
        }
    }
}