namespace EA.Iws.Domain.NotificationApplication.Shipment
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ShipmentNumberHistory : Entity
    {
        protected ShipmentNumberHistory()
        {
        }

        public ShipmentNumberHistory(Guid notificationId, int numberOfShipments, DateTime dateChanged)
        {
            Guard.ArgumentNotDefaultValue(() => notificationId, notificationId);
            Guard.ArgumentNotDefaultValue(() => dateChanged, dateChanged);
            Guard.ArgumentNotZeroOrNegative(() => numberOfShipments, numberOfShipments);

            NotificationId = notificationId;
            NumberOfShipments = numberOfShipments;
            DateChanged = dateChanged;
        }

        public Guid NotificationId { get; private set; }

        public int NumberOfShipments { get; private set; }

        public DateTime DateChanged { get; private set; }
    }
}
