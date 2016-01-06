namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using NotificationConsent;

    public class ConsentPeriod
    {
        private readonly INotificationConsentRepository consentRepository;

        public ConsentPeriod(INotificationConsentRepository consentRepository)
        {
            this.consentRepository = consentRepository;
        }

        public async Task<bool> HasExpired(Guid notificationId)
        {
            var consentEndDate = (await consentRepository.GetByNotificationId(notificationId)).ConsentRange.To;

            return consentEndDate < DateTime.UtcNow;
        }
    }
}
