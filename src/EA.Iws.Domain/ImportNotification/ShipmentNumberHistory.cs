namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ShipmentNumberHistory : Entity
    {
        protected ShipmentNumberHistory()
        {
        }

        public ShipmentNumberHistory(Guid importNotificationId, int numberOfShipments, DateTime dateChanged)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);
            Guard.ArgumentNotDefaultValue(() => dateChanged, dateChanged);
            Guard.ArgumentNotZeroOrNegative(() => numberOfShipments, numberOfShipments);

            ImportNotificationId = importNotificationId;
            NumberOfShipments = numberOfShipments;
            DateChanged = dateChanged;
        }

        public Guid ImportNotificationId { get; private set; }

        public int NumberOfShipments { get; private set; }

        public DateTime DateChanged { get; private set; }
    }
}
