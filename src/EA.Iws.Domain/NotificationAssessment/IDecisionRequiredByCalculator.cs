namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Core.Notification;

    public interface IDecisionRequiredByCalculator
    {
        DateTime Get(bool areFacilitiesPreconsented, DateTime acknowledgedDate, UKCompetentAuthority competentAuthority);
    }
}
