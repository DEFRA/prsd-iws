namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using NotificationApplication;
    using NotificationConsent;
    using Prsd.Core;

    public class ConsentPeriod
    {
        private readonly INotificationConsentRepository consentRepository;
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public ConsentPeriod(INotificationConsentRepository consentRepository,
            IWorkingDayCalculator workingDayCalculator,
            INotificationApplicationRepository notificationApplicationRepository)
        {
            this.consentRepository = consentRepository;
            this.workingDayCalculator = workingDayCalculator;
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<bool> HasExpired(Guid notificationId)
        {
            var consentEndDate = (await consentRepository.GetByNotificationId(notificationId)).ConsentRange.To;

            return consentEndDate < SystemTime.UtcNow;
        }

        public async Task<bool> ExpiresInFourWorkingDays(Guid notificationId)
        {
            return (await GetWorkingDaysToExpiry(notificationId)) == 4;
        }

        public async Task<bool> ExpiresInThreeOrLessWorkingDays(Guid notificationId)
        {
            return (await GetWorkingDaysToExpiry(notificationId)) < 4 && !(await HasExpired(notificationId));
        }

        private async Task<int> GetWorkingDaysToExpiry(Guid notificationId)
        {
            var ca = (await notificationApplicationRepository.GetById(notificationId)).CompetentAuthority;
            var consentEndDate = (await consentRepository.GetByNotificationId(notificationId)).ConsentRange.To;

            return workingDayCalculator.GetWorkingDays(SystemTime.UtcNow, consentEndDate, true, ca);
        }
    }
}