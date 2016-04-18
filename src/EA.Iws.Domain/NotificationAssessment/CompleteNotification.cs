namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using NotificationApplication;

    [AutoRegister]
    public class CompleteNotification
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IFacilityRepository facilityRepository;
        private readonly INotificationTransactionCalculator notificationTransactionCalculator;

        public CompleteNotification(INotificationAssessmentRepository notificationAssessmentRepository, IFacilityRepository facilityRepository,
            INotificationTransactionCalculator notificationTransactionCalculator)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.facilityRepository = facilityRepository;
            this.notificationTransactionCalculator = notificationTransactionCalculator;
        }

        public async Task Complete(Guid notificationId, DateTime completedDate)
        {
            if (!await IsInterimSet(notificationId))
            {
                throw new InvalidOperationException(
                    string.Format("Can't complete notification {0} as IsInterim has not been set.", notificationId));
            }

            if (!await IsPaymentComplete(notificationId))
            {
                throw new InvalidOperationException(
                    string.Format("Can't complete notification {0} as payment is not fully made.", notificationId));
            }

            var assessment = await notificationAssessmentRepository.GetByNotificationId(notificationId);
            assessment.Complete(completedDate);
        }

        public async Task<bool> CanComplete(Guid notificationId)
        {
            return await IsInterimSet(notificationId)
                   && await IsPaymentComplete(notificationId);
        }

        private async Task<bool> IsInterimSet(Guid notificationId)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(notificationId);
            return facilityCollection.IsInterim.HasValue;
        }

        private async Task<bool> IsPaymentComplete(Guid notificationId)
        {
            return await notificationTransactionCalculator.IsPaymentComplete(notificationId);
        }
    }
}