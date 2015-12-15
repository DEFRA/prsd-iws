namespace EA.Iws.Domain.NotificationAssessment
{
    using System;

    public interface IDecisionRequiredByCalculator
    {
        DateTime Get(bool areFacilitiesPreconsented, DateTime acknowledgedDate, UKCompetentAuthority competentAuthority);
    }
}
