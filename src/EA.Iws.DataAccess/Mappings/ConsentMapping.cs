namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationAssessment;

    internal class ConsentMapping : EntityTypeConfiguration<Consent>
    {
        public ConsentMapping()
        {
            this.ToTable("Consent", "Notification");

            Property(x => x.Conditions).HasMaxLength(4000);
        }
    }
}
