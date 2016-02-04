namespace EA.Iws.Domain
{
    using System;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public interface IWorkingDayCalculator
    {
        int GetWorkingDays(DateTime start, DateTime end, bool includeStartDay, CompetentAuthorityEnum competentAuthority = CompetentAuthorityEnum.England);

        DateTime AddWorkingDays(DateTime start, int days, bool includeStartDay, CompetentAuthorityEnum competentAuthority = CompetentAuthorityEnum.England);
    }
}
