namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ShipmentInfo : Entity
    {
        private decimal quantity;

        protected ShipmentInfo()
        {
        }

        public ShipmentInfo(Guid notificationId, ShipmentPeriod shipmentPeriod, int numberOfShipments, ShipmentQuantity shipmentQuantity)
        {
            NotificationId = notificationId;
            UpdateQuantity(shipmentQuantity);
            UpdateShipmentPeriod(shipmentPeriod);
            UpdateNumberOfShipments(numberOfShipments);
        }

        public Guid NotificationId { get; private set; }

        public int NumberOfShipments { get; private set; }

        public ShipmentPeriod ShipmentPeriod { get; private set; }

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

        public void UpdateQuantity(ShipmentQuantity shipmentQuantity)
        {
            Quantity = shipmentQuantity.Quantity;
            Units = shipmentQuantity.Units;
        }

        public void UpdateShipmentPeriod(ShipmentPeriod shipmentPeriod)
        {
            if (shipmentPeriod.FirstDate < SystemTime.Now.Date)
            {
                throw new InvalidOperationException(string.Format(
                    "The start date cannot be in the past on shipment info {0}", Id));
            }

            ShipmentPeriod = shipmentPeriod;
        }

        public void UpdateNumberOfShipments(int numberOfShipments)
        {
            Guard.ArgumentNotZeroOrNegative(() => numberOfShipments, numberOfShipments);

            NumberOfShipments = numberOfShipments;
        }
    }
}