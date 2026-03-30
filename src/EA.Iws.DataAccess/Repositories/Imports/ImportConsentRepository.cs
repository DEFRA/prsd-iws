namespace EA.Iws.DataAccess.Repositories.Imports
{
    using Domain.ImportNotificationAssessment.Consent;
    using EA.Iws.Domain.Security;
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;

    internal class ImportConsentRepository : IImportConsentRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportConsentRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public void Add(ImportConsent consent)
        {
            context.ImportConsents.Add(consent);
        }

        public async Task<ImportConsent> GetByNotificationIdOrDefault(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.ImportConsents.SingleOrDefaultAsync(c => c.NotificationId == notificationId);
        }
    }
}
