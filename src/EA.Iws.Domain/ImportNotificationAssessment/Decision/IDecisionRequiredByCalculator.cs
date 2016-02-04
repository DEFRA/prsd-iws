namespace EA.Iws.Domain.ImportNotificationAssessment.Decision
{
    using System;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public interface IDecisionRequiredByCalculator
    {
        DateTime Get(bool areFacilitiesPreconsented, DateTime acknowledgedDate, CompetentAuthorityEnum competentAuthority);
    }
}
