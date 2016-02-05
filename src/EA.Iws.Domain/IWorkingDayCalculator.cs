namespace EA.Iws.Domain
{
    using System;
    using Core.Notification;

    public interface IWorkingDayCalculator
    {
        int GetWorkingDays(DateTime start, DateTime end, bool includeStartDay, UKCompetentAuthority competentAuthority = UKCompetentAuthority.England);

        DateTime AddWorkingDays(DateTime start, int days, bool includeStartDay, UKCompetentAuthority competentAuthority = UKCompetentAuthority.England);
    }
}
