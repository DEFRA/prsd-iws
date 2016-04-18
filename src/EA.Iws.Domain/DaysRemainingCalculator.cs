namespace EA.Iws.Domain
{
    using System;
    using Core.ComponentRegistration;
    using Prsd.Core;

    [AutoRegister]
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