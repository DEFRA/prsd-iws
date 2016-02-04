namespace EA.Iws.RequestHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.ComponentRegistration;
    using DataAccess;
    using Domain;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    /// <summary>
    ///     Adaptation of: http://stackoverflow.com/a/1619375/1775471
    /// </summary>
    [AutoRegister]
    internal class WorkingDayCalculator : IWorkingDayCalculator
    {
        private readonly IList<BankHoliday> bankHolidays;

        public WorkingDayCalculator(IwsContext context)
        {
            this.bankHolidays = context.BankHolidays.ToArray();
        }

        public int GetWorkingDays(DateTime start, DateTime end, bool includeStartDay,
            CompetentAuthorityEnum competentAuthority = CompetentAuthorityEnum.England)
        {
            start = start.Date;
            end = end.Date;

            var negativeTimeSpan = start > end;

            var workingDays = negativeTimeSpan
                ? GetWorkingDaysLogic(end, start, competentAuthority)
                : GetWorkingDaysLogic(start, end, competentAuthority);

            if (!includeStartDay)
            {
                workingDays = AdjustForStartDate(workingDays, start, end, competentAuthority);
            }

            return negativeTimeSpan ? -1 * workingDays : workingDays;
        }

        public DateTime AddWorkingDays(DateTime start, int days, bool includeStartDay,
            CompetentAuthorityEnum competentAuthority = CompetentAuthorityEnum.England)
        {
            if (days == 0)
            {
                return start;
            }

            var negativeDays = days < 0;

            if (includeStartDay && !(IsAWeekend(start) || IsBankHoliday(start, competentAuthority)))
            {
                if (days > 0)
                {
                    days--;
                }
                else if (negativeDays)
                {
                    days++;
                }
            }

            var iteratedDate = start;

            while (days != 0)
            {
                iteratedDate = negativeDays ? iteratedDate.AddDays(-1) : iteratedDate.AddDays(1);

                if (!IsAWeekend(iteratedDate) && !IsBankHoliday(iteratedDate, competentAuthority))
                {
                    days += negativeDays ? 1 : -1;
                }
            }

            return iteratedDate;
        }

        private int GetWorkingDaysLogic(DateTime start, DateTime end, CompetentAuthorityEnum competentAuthority)
        {
            var dateSpan = (end - start);
            var workingDays = 1 + dateSpan.Days;

            var numberOfWeeks = workingDays / 7;

            if (workingDays > numberOfWeeks * 7)
            {
                var firstDayOfWeekValue = IsASunday(start) ? 7 : (int)start.DayOfWeek;
                var lastDayOfWeekValue = IsASunday(end) ? 7 : (int)end.DayOfWeek;

                if (lastDayOfWeekValue < firstDayOfWeekValue)
                {
                    // Add a week.
                    lastDayOfWeekValue += 7;
                }

                if (!IsASunday(firstDayOfWeekValue))
                {
                    // Sunday and Saturday fall within this period.
                    if (lastDayOfWeekValue >= 7)
                    {
                        workingDays -= 2;
                    }
                    else if (lastDayOfWeekValue == 6)
                    {
                        // Saturday is within this period.
                        workingDays -= 1;
                    }
                }
                else if (IsASunday(firstDayOfWeekValue) && lastDayOfWeekValue >= 7)
                {
                    workingDays--;
                }
            }

            // Remove the weekends.
            workingDays -= numberOfWeeks * 2;

            workingDays = RemoveBankHolidays(start, end, workingDays, competentAuthority);

            return workingDays;
        }

        private bool IsASunday(int dayValue)
        {
            return dayValue == 7 || dayValue == (int)DayOfWeek.Sunday;
        }

        private bool IsASunday(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday;
        }

        private int AdjustForStartDate(int dayDifference, DateTime start, DateTime end,
            CompetentAuthorityEnum competentAuthority)
        {
            if (IsAWeekend(start) || IsBankHoliday(start, competentAuthority))
            {
                return dayDifference;
            }

            if (dayDifference > 0)
            {
                dayDifference--;
            }
            else if (dayDifference < 0)
            {
                dayDifference++;
            }
            else if (dayDifference == 0 && start == end)
            {
                dayDifference++;
            }

            return dayDifference;
        }

        private bool IsBankHoliday(DateTime date, CompetentAuthorityEnum competentAuthority)
        {
            return bankHolidays.Where(bh => bh.CompetentAuthority == competentAuthority).Select(bh => bh.Date.Date).Contains(date, new CustomDateComparer());
        }

        private int RemoveBankHolidays(DateTime start, DateTime end, int workingDays,
            CompetentAuthorityEnum competentAuthority)
        {
            foreach (var bankHoliday in bankHolidays.Where(bh => bh.CompetentAuthority == competentAuthority).Select(bh => bh.Date))
            {
                if (start <= bankHoliday.Date && bankHoliday.Date <= end)
                {
                    workingDays--;
                }
            }

            return workingDays;
        }

        private bool IsAWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;
        }

        private class CustomDateComparer : IEqualityComparer<DateTime>
        {
            public bool Equals(DateTime x, DateTime y)
            {
                return x.Day == y.Day && x.Month == y.Month && x.Year == y.Year;
            }

            public int GetHashCode(DateTime obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}