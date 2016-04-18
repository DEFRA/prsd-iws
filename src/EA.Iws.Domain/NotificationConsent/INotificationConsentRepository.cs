namespace EA.Iws.Domain.NotificationConsent
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationConsentRepository
    {
        void Add(Consent consent);

        Task<Consent> GetByNotificationId(Guid notificationId);
    }
}
