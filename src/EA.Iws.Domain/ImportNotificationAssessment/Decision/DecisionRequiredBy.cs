namespace EA.Iws.Domain.ImportNotificationAssessment.Decision
{
    using System;
    using System.Threading.Tasks;
    using ImportNotification;
    using Prsd.Core;

    public class DecisionRequiredBy
    {
        private readonly IImportNotificationRepository notificationRepository;
        private readonly IFacilityRepository facilityRepository;
        private readonly IDecisionRequiredByCalculator decisionRequiredByCalculator;

        public DecisionRequiredBy(IImportNotificationRepository notificationRepository, 
            IFacilityRepository facilityRepository, 
            IDecisionRequiredByCalculator decisionRequiredByCalculator)
        {
            this.notificationRepository = notificationRepository;
            this.facilityRepository = facilityRepository;
            this.decisionRequiredByCalculator = decisionRequiredByCalculator;
        }

        public async Task<DateTime?> GetDecisionRequiredByDate(ImportNotificationAssessment notificationAssessment)
        {
            Guard.ArgumentNotNull(() => notificationAssessment, notificationAssessment);

            var notification = await notificationRepository.Get(notificationAssessment.NotificationApplicationId);
            var facilityCollection =
                await facilityRepository.GetByNotificationId(notificationAssessment.NotificationApplicationId);

            if (!notificationAssessment.Dates.AcknowledgedDate.HasValue)
            {
                return null;
            }

            return
                decisionRequiredByCalculator.Get(
                    facilityCollection.AllFacilitiesPreconsented,
                    notificationAssessment.Dates.AcknowledgedDate.Value.DateTime,
                    notification.CompetentAuthority);
        }
    }
}
