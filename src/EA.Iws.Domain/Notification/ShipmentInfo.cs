namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ShipmentInfo : Entity
    {
        public int NumberOfShipments { get; set; }
        public decimal Quantity { get; set; }
        public ShipmentQuantityUnits Units { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }

        public ShipmentInfo()
        {
        }

        public ShipmentInfo(int numberOfShipments, 
            decimal quantity, 
            ShipmentQuantityUnits units, 
            DateTime firstDate,
            DateTime lastDate)
        {
            Guard.ArgumentNotNull(units);

            NumberOfShipments = numberOfShipments;
            Quantity = quantity;
            Units = units;
            FirstDate = firstDate;
            LastDate = lastDate;
        }
    }
}
