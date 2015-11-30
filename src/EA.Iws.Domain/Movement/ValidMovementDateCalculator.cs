namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using NotificationApplication;
    using NotificationConsent;

    public class ValidMovementDateCalculator
    {
        private readonly IMovementRepository movementRepository;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly OriginalMovementDate originalDateService;
        private readonly INotificationConsentRepository consentRepository;
        private readonly IWorkingDayCalculator workingDayCalculator;

        public ValidMovementDateCalculator(IMovementRepository movementRepository,
            INotificationApplicationRepository notificationRepository,
            INotificationConsentRepository consentRepository,
            OriginalMovementDate originalDateService,
            IWorkingDayCalculator workingDayCalculator)
        {
            this.workingDayCalculator = workingDayCalculator;
            this.consentRepository = consentRepository;
            this.originalDateService = originalDateService;
            this.notificationRepository = notificationRepository;
            this.movementRepository = movementRepository;
        }

        public async Task<DateTime> Maximum(Guid movementId)
        {
            var movement = await movementRepository.GetById(movementId);
            var originalDate = await originalDateService.Get(movement);
            var competentAuthority = (await notificationRepository.GetByMovementId(movementId)).CompetentAuthority;
            var consent = await consentRepository.GetByNotificationId(movement.NotificationId);

            var result = workingDayCalculator.AddWorkingDays(originalDate, 10, false, competentAuthority);
            var consentEndDate = consent.ConsentRange.To;

            if (result > consentEndDate)
            {
                return consentEndDate;
            }

            return result;
        }
    }
}