namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using NotificationApplication;
    using Prsd.Core;

    public class DecisionRequiredBy
    {
        private readonly IDecisionRequiredByCalculator decisionRequiredByCalculator;
        private readonly IFacilityRepository facilityRepository;

        public DecisionRequiredBy(IDecisionRequiredByCalculator decisionRequiredByCalculator, IFacilityRepository facilityRepository)
        {
            this.decisionRequiredByCalculator = decisionRequiredByCalculator;
            this.facilityRepository = facilityRepository;
        }

        public async Task<DateTime?> GetDecisionRequiredByDate(NotificationApplication notificationApplication,
            NotificationAssessment notificationAssessment)
        {
            Guard.ArgumentNotNull(() => notificationApplication, notificationApplication);
            Guard.ArgumentNotNull(() => notificationAssessment, notificationAssessment);

            if (!notificationAssessment.Dates.AcknowledgedDate.HasValue)
            {
                return null;
            }

            var facilityCollection = await facilityRepository.GetByNotificationId(notificationApplication.Id);

            return
                decisionRequiredByCalculator.Get(
                    facilityCollection.AllFacilitiesPreconsented.GetValueOrDefault(),
                    notificationAssessment.Dates.AcknowledgedDate.Value,
                    notificationApplication.CompetentAuthority);
        }
    }
}
