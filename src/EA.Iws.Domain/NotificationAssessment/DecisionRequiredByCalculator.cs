namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Core.ComponentRegistration;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    [AutoRegister]
    public class DecisionRequiredByCalculator : IDecisionRequiredByCalculator
    {
        private readonly IWorkingDayCalculator workingDayCalculator;

        public DecisionRequiredByCalculator(IWorkingDayCalculator workingDayCalculator)
        {
            this.workingDayCalculator = workingDayCalculator;
        }

        public DateTime Get(bool areFacilitiesPreconsented, DateTime acknowledgedDate, CompetentAuthorityEnum competentAuthority)
        {
            if (areFacilitiesPreconsented)
            {
                return workingDayCalculator.AddWorkingDays(acknowledgedDate,
                    7,
                    false,
                    competentAuthority);
            }

            return acknowledgedDate.AddDays(30);
        }
    }
}