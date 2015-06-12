namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ShipmentInfo : Entity
    {
        private int? numberOfShipments;
        private decimal? quantity;

        internal ShipmentInfo()
        {
            Units = ShipmentQuantityUnits.NotSet;
        }

        public int? NumberOfShipments
        {
            get { return numberOfShipments; }
            internal set
            {
                if (value.HasValue)
                {
                    Guard.ArgumentNotZeroOrNegative(() => value.Value, value.Value);
                }
                numberOfShipments = value;
            }
        }

        public DateTime FirstDate { get; private set; }

        public DateTime LastDate { get; private set; }

        public ShipmentQuantityUnits Units { get; internal set; }

        public decimal? Quantity
        {
            get { return quantity; }
            internal set
            {
                if (value.HasValue)
                {
                    Guard.ArgumentNotZeroOrNegative(() => value.Value, value.Value);
                    quantity = decimal.Round(value.Value, 4, MidpointRounding.AwayFromZero);
                }
                else
                {
                    quantity = null;
                }
            }
        }

        internal void UpdateShipmentDates(DateTime firstDate, DateTime lastDate)
        {
            Guard.ArgumentNotDefaultValue(() => firstDate, firstDate);
            Guard.ArgumentNotDefaultValue(() => lastDate, lastDate);

            if (firstDate > lastDate)
            {
                throw new InvalidOperationException(
                    string.Format("The start date must be before the end date on shipment info {0}", Id));
            }

            FirstDate = firstDate;
            LastDate = lastDate;
        }
    }
}