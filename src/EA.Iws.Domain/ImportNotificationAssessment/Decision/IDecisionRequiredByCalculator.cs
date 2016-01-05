namespace EA.Iws.Domain.ImportNotificationAssessment.Decision
{
    using System;

    public interface IDecisionRequiredByCalculator
    {
        DateTime Get(bool areFacilitiesPreconsented, DateTime acknowledgedDate, UKCompetentAuthority competentAuthority);
    }
}
