namespace EA.Iws.Domain.ImportNotificationAssessment.Decision
{
    using System;
    using Core.ComponentRegistration;
    using Core.Notification;

    [AutoRegister]
    public class DecisionRequiredByCalculator : IDecisionRequiredByCalculator
    {
        private readonly IWorkingDayCalculator workingDayCalculator;

        public DecisionRequiredByCalculator(IWorkingDayCalculator workingDayCalculator)
        {
            this.workingDayCalculator = workingDayCalculator;
        }

        public DateTime Get(bool areFacilitiesPreconsented, DateTime acknowledgedDate, UKCompetentAuthority competentAuthority)
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