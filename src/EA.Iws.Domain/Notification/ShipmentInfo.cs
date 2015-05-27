namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ShipmentInfo : Entity
    {
        private int numberOfShipments;
        private decimal quantity;
        private ShipmentQuantityUnits units;
        private DateTime firstDate;
        private DateTime lastDate;

        protected ShipmentInfo()
        {
        }

        internal ShipmentInfo(int numberOfShipments, 
            decimal quantity, 
            ShipmentQuantityUnits units, 
            DateTime firstDate,
            DateTime lastDate)
        {
            NumberOfShipments = numberOfShipments;
            Quantity = quantity;
            Units = units;
            FirstDate = firstDate;
            LastDate = lastDate;
        }

        public int NumberOfShipments
        {
            get { return numberOfShipments; }
            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("The number of shipments must be greater than zero.");
                }
                numberOfShipments = value;
            }
        }

        public DateTime FirstDate
        {
            get { return firstDate; }
            set
            {
                if (value >= LastDate && LastDate != DateTime.MinValue)
                {
                    throw new InvalidOperationException("The start date must be before the end date");
                }
                firstDate = value;
            }
        }

        public DateTime LastDate
        {
            get { return lastDate; }
            set
            {
                if (value <= FirstDate && FirstDate != DateTime.MinValue)
                {
                    throw new InvalidOperationException("The end date must be after the start date");
                }
                lastDate = value;
            }
        }

        public ShipmentQuantityUnits Units
        {
            get { return units; }
            set
            {
                Guard.ArgumentNotNull(value);
                units = value;
            }
        }
        
        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("The quantity entered must be greater than zero.");
                }
                quantity = value;
            }
        }
    }
}
