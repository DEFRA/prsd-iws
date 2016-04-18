namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment.Consent;

    internal class ImportConsentMapping : EntityTypeConfiguration<ImportConsent>
    {
        public ImportConsentMapping()
        {
            ToTable("Consent", "ImportNotification");

            Property(x => x.Conditions).HasMaxLength(4000);
        }
    }
}
