namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using NotificationApplication;
    using Prsd.Core;

    public class DecisionRequiredBy
    {
        private readonly IDecisionRequiredByCalculator decisionRequiredByCalculator;

        public DecisionRequiredBy(IDecisionRequiredByCalculator decisionRequiredByCalculator)
        {
            this.decisionRequiredByCalculator = decisionRequiredByCalculator;
        }

        public DateTime? GetDecisionRequiredByDate(NotificationApplication notificationApplication,
            NotificationAssessment notificationAssessment)
        {
            Guard.ArgumentNotNull(() => notificationApplication, notificationApplication);
            Guard.ArgumentNotNull(() => notificationAssessment, notificationAssessment);

            if (!notificationAssessment.Dates.AcknowledgedDate.HasValue)
            {
                return null;
            }

            return
                decisionRequiredByCalculator.Get(
                    notificationApplication.IsPreconsentedRecoveryFacility.GetValueOrDefault(),
                    notificationAssessment.Dates.AcknowledgedDate.Value,
                    notificationApplication.CompetentAuthority);
        }
    }
}
