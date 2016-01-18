namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using NotificationApplication;

    public class CompleteNotification
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IFacilityRepository facilityRepository;

        public CompleteNotification(INotificationAssessmentRepository notificationAssessmentRepository, IFacilityRepository facilityRepository)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.facilityRepository = facilityRepository;
        }

        public async Task Complete(Guid notificationId, DateTime completedDate)
        {
            if (await CanComplete(notificationId))
            {
                var assessment = await notificationAssessmentRepository.GetByNotificationId(notificationId);
                assessment.Complete(completedDate);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Can't complete notification {0} as IsInterim has not been set.", notificationId));
            }
        }

        public async Task<bool> CanComplete(Guid notificationId)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(notificationId);
            return facilityCollection.IsInterim.HasValue;
        }
    }
}