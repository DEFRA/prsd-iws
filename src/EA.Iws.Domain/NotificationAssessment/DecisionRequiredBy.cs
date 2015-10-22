namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using NotificationApplication;
    using Prsd.Core;

    public class DecisionRequiredBy
    {
        private readonly IWorkingDayCalculator workingDayCalculator;

        public DecisionRequiredBy(IWorkingDayCalculator workingDayCalculator)
        {
            this.workingDayCalculator = workingDayCalculator;
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

            if (notificationApplication.IsPreconsentedRecoveryFacility
                .GetValueOrDefault())
            {
                return workingDayCalculator.AddWorkingDays(notificationAssessment.Dates.AcknowledgedDate.Value,
                    7,
                    false,
                    notificationApplication.CompetentAuthority);
            }

            return notificationAssessment.Dates.AcknowledgedDate.Value.AddDays(30);
        }
    }
}
