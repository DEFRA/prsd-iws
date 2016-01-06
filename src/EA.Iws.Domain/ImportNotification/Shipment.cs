namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Shipment : Entity
    {
        public Guid ImportNotificationId { get; private set; }

        public ShipmentPeriod Period { get; private set; }

        public ShipmentQuantity Quantity { get; private set; }

        public int NumberOfShipments { get; private set; }

        protected Shipment()
        {
        }

        public Shipment(Guid importNotificationId, ShipmentPeriod period, ShipmentQuantity quantity, int numberOfShipments)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);
            Guard.ArgumentNotNull(() => period, period);
            Guard.ArgumentNotNull(() => quantity, quantity);
            Guard.ArgumentNotZeroOrNegative(() => numberOfShipments, numberOfShipments);

            ImportNotificationId = importNotificationId;
            Period = period;
            Quantity = quantity;
            NumberOfShipments = numberOfShipments;
        }
    }
}