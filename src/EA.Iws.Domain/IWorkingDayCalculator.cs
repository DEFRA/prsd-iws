namespace EA.Iws.Domain
{
    using System;

    public interface IWorkingDayCalculator
    {
        int GetWorkingDays(DateTime start, DateTime end, bool includeStartDay, UKCompetentAuthority competentAuthority = null);

        DateTime AddWorkingDays(DateTime start, int days, bool includeStartDay, UKCompetentAuthority competentAuthority = null);
    }
}
