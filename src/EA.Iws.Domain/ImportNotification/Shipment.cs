namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Shipment : Entity
    {
        public Guid ImportNotificationId { get; set; }

        public ShipmentPeriod Period { get; set; }

        public ShipmentQuantity Quantity { get; set; }

        public int NumberOfShipments { get; set; }

        protected Shipment()
        {
        }

        public Shipment(Guid importNotificationId, ShipmentPeriod period, ShipmentQuantity quantity, int numberOfShipments)
        {
            ImportNotificationId = importNotificationId;
            Period = period;
            Quantity = quantity;

            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);
            Guard.ArgumentNotNull(() => period, period);
            Guard.ArgumentNotNull(() => quantity, quantity);
            Guard.ArgumentNotZeroOrNegative(() => numberOfShipments, numberOfShipments);
        }
    }
}