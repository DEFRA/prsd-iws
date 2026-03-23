namespace EA.Iws.Domain.ImportNotificationAssessment.Consent
{
    using System;
    using System.Threading.Tasks;

    public interface IImportConsentRepository
    {
        void Add(ImportConsent consent);

        Task<ImportConsent> GetByNotificationIdOrDefault(Guid notificationId);
    }
}
