namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ShipmentInfo : Entity
    {
        private int numberOfShipments;
        private decimal quantity;

        protected ShipmentInfo()
        {
        }

        internal ShipmentInfo(DateTime firstDate, DateTime lastDate, int numberOfShipments, decimal quantity, ShipmentQuantityUnits unit)
        {
            FirstDate = firstDate;
            LastDate = lastDate;
            NumberOfShipments = numberOfShipments;
            Quantity = quantity;
            Units = unit;
        }

        public int NumberOfShipments
        {
            get { return numberOfShipments; }
            internal set
            {
                Guard.ArgumentNotZeroOrNegative(() => value, value);
                numberOfShipments = value;
            }
        }

        public DateTime FirstDate { get; private set; }

        public DateTime LastDate { get; private set; }

        public ShipmentQuantityUnits Units { get; internal set; }

        public decimal Quantity
        {
            get { return quantity; }
            internal set
            {
                Guard.ArgumentNotZeroOrNegative(() => value, value);
                quantity = decimal.Round(value, 4, MidpointRounding.AwayFromZero);
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