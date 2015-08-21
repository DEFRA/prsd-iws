namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ShipmentInfo : Entity
    {
        private int numberOfShipments;
        private decimal quantity;
        private DateTime firstDate;
        private DateTime lastDate;

        protected ShipmentInfo()
        {
        }

        internal ShipmentInfo(DateTime firstDate, DateTime lastDate, int numberOfShipments, decimal quantity, ShipmentQuantityUnits unit)
        {
            UpdateShipmentPeriod(firstDate, lastDate);
            UpdateQuantity(quantity, unit);

            NumberOfShipments = numberOfShipments;
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

        public DateTime FirstDate
        {
            get { return firstDate; }
            private set
            {
                Guard.ArgumentNotDefaultValue(() => value, value);
                firstDate = value;
            }
        }

        public DateTime LastDate
        {
            get { return lastDate; }
            private set
            {
                Guard.ArgumentNotDefaultValue(() => value, value);
                lastDate = value;
            }
        }

        public ShipmentQuantityUnits Units { get; private set; }

        public decimal Quantity
        {
            get { return quantity; }
            private set
            {
                Guard.ArgumentNotZeroOrNegative(() => value, value);
                quantity = decimal.Round(value, 4, MidpointRounding.AwayFromZero);
            }
        }

        internal void UpdateShipmentPeriod(DateTime firstDate, DateTime lastDate)
        {
            Guard.ArgumentNotDefaultValue(() => firstDate, firstDate);
            Guard.ArgumentNotDefaultValue(() => lastDate, lastDate);

            if (firstDate < SystemTime.Now.Date)
            {
                throw new InvalidOperationException(string.Format("The start date cannot be in the past on shipment info {0}", Id));
            }

            if (firstDate > lastDate)
            {
                throw new InvalidOperationException(
                    string.Format("The start date must be before the end date on shipment info {0}", Id));
            }

            this.firstDate = firstDate;
            this.lastDate = lastDate;
        }

        internal void UpdateQuantity(decimal quantity, ShipmentQuantityUnits units)
        {
            Quantity = quantity;
            Units = units;
        }
    }
}