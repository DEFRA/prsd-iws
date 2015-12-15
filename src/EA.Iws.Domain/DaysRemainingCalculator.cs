namespace EA.Iws.Domain
{
    using System;
    using Prsd.Core;

    public class DaysRemainingCalculator
    {
        public string Calculate(DateTime targetDate)
        {
            var today = SystemTime.UtcNow;

            if (targetDate.Date < today.Date)
            {
                return "Overdue";
            }

            var diff = targetDate.Date.Subtract(today.Date);

            return diff.Days.ToString();
        }
    }
}