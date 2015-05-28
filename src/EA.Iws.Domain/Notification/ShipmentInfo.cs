namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ShipmentInfo : Entity
    {
        private int numberOfShipments;
        private decimal quantity;
        private ShipmentQuantityUnits units;
        private DateTime firstDate;
        private DateTime lastDate;

        public virtual List<PackagingType> PackagingTypes { get; set; }

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
                Guard.ArgumentNotZeroOrNegative(() => value, value);
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
                Guard.ArgumentNotZeroOrNegative(() => value, value);
                quantity = value;
            }
        }

        public bool IsSpecialHandling { get; internal set; }

        public string SpecialHandlingDetails { get; set; }
    }
}
