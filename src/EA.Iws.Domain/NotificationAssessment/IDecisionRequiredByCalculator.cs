namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public interface IDecisionRequiredByCalculator
    {
        DateTime Get(bool areFacilitiesPreconsented, DateTime acknowledgedDate, CompetentAuthorityEnum competentAuthority);
    }
}
