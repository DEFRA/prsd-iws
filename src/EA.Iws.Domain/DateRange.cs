namespace EA.Iws.Domain
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

        protected DateRange()
        {
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

        public bool Contains(DateTime date)
        {
            return date >= From && date <= To;
        }
    }
}
