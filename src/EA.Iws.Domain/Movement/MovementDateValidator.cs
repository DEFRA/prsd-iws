namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using NotificationApplication;
    using NotificationConsent;
    using Prsd.Core;

    public class MovementDateValidator : IMovementDateValidator
    {
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly INotificationConsentRepository consentRepository;

        public MovementDateValidator(INotificationConsentRepository consentRepository,
            INotificationApplicationRepository notificationRepository,
            IWorkingDayCalculator workingDayCalculator)
        {
            this.consentRepository = consentRepository;
            this.notificationRepository = notificationRepository;
            this.workingDayCalculator = workingDayCalculator;
        }

        public async Task EnsureDateValid(Guid notificationId, DateTime date)
        {
            var consent = await consentRepository.GetByNotificationId(notificationId);

            if (!consent.ConsentRange.Contains(date))
            {
                throw new MovementDateException(string.Format(
                    "Can't set new movement date {0} since it is outside the consent range for this notification",
                    date));
            }

            if (date < SystemTime.UtcNow)
            {
                throw new MovementDateException(string.Format(
                    "Can't set new movement date {0} since it is in the past",
                    date));
            }

            var includeStartDay = false;
            var notification = await notificationRepository.GetById(notificationId);
            if (date > workingDayCalculator.AddWorkingDays(SystemTime.UtcNow, 30, includeStartDay, notification.CompetentAuthority))
            {
                throw new MovementDateException(string.Format(
                    "Can't set new movement date {0} since it is more than 30 working days in the future",
                    date));
            }
        }
    }
}