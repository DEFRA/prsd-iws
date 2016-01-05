namespace EA.Iws.DataAccess.Repositories.Imports
{
    using Domain.ImportNotificationAssessment.Consent;

    internal class ImportConsentRepository : IImportConsentRepository
    {
        private readonly ImportNotificationContext context;

        public ImportConsentRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public void Add(ImportConsent consent)
        {
            context.ImportConsents.Add(consent);
        }
    }
}
