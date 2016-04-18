namespace EA.Iws.Domain
{
    using System;

    public class DateTimeOffsetRange
    {
        public DateTimeOffset From { get; private set; }

        public DateTimeOffset To { get; private set; }

        public int Days
        {
            get { return (To - From).Days; }
        }

        protected DateTimeOffsetRange()
        {
        }

        public DateTimeOffsetRange(DateTimeOffset from, DateTimeOffset to)
        {
            if (from.Date > to.Date)
            {
                throw new InvalidOperationException("Cannot create a date range with the from date after the to date.");
            }

            From = from;
            To = to;
        }

        public bool Contains(DateTimeOffset date)
        {
            if (date > this.From && date < this.To)
            {
                return true;
            }

            return false;
        }
    }
}
