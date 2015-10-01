namespace EA.Iws.Core.Shared
{
    using System;

    public class DateRange
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int Days
        {
            get { return (To - From).Days; }
        }

        public DateRange(DateTime from, DateTime to)
        {
            if (from.Date > to.Date)
            {
                throw new InvalidOperationException("Cannot create a date range with the from date after the to date.");
            }

            From = from;
            To = to;
        }
    }
}
