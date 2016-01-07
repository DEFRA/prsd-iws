namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using NotificationApplication;
    using NotificationConsent;
    using Prsd.Core;

    public class MovementDateValidator : IMovementDateValidator
    {
        private readonly INotificationConsentRepository consentRepository;

        public MovementDateValidator(INotificationConsentRepository consentRepository)
        {
            this.consentRepository = consentRepository;
        }

        public async Task EnsureDateValid(Guid notificationId, DateTime date)
        {
            var consent = await consentRepository.GetByNotificationId(notificationId);

            if (!consent.ConsentRange.Contains(date))
            {
                throw new MovementDateOutsideConsentPeriodException(string.Format(
                    "Can't set new movement date {0} since it is outside the consent range for this notification",
                    date));
            }

            if (date < SystemTime.UtcNow)
            {
                throw new MovementDateException(string.Format(
                    "Can't set new movement date {0} since it is in the past",
                    date));
            }

            if (date > SystemTime.UtcNow.AddDays(30))
            {
                throw new MovementDateOutOfRangeException(string.Format(
                    "Can't set new movement date {0} since it is more than 30 days in the future",
                    date));
            }
        }
    }
}