namespace EA.Iws.Domain
{
    using Prsd.Core;
    using System;

    public class ShipmentPeriod
    {
        public DateTime FirstDate { get; private set; }
        public DateTime LastDate { get; private set; }

        protected ShipmentPeriod()
        {
        }

        public ShipmentPeriod(DateTime firstDate, DateTime lastDate, bool isPreconsentedRecoveryFacility)
        {
            Guard.ArgumentNotDefaultValue(() => firstDate, firstDate);
            Guard.ArgumentNotDefaultValue(() => lastDate, lastDate);

            if (firstDate > lastDate)
            {
                throw new InvalidOperationException(
                    "The start date must be before the end date in a shipment period");
            }

            int maxPeriodLengthMonths = isPreconsentedRecoveryFacility ? 36 : 12;

            if (lastDate >= firstDate.AddMonths(maxPeriodLengthMonths))
            {
                throw new InvalidOperationException(string.Format("The start date and end date must be within a {0} month period.", maxPeriodLengthMonths));
            }

            FirstDate = firstDate;
            LastDate = lastDate;
        }

        public bool IsDateInShipmentPeriod(DateTime date)
        {
            if (date <= LastDate && date >= FirstDate)
            {
                return true;
            }

            return false;
        }
    }
}
