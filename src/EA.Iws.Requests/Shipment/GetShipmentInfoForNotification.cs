namespace EA.Iws.Requests.Shipment
{
    using System;
    using Prsd.Core.Mediator;

    public class GetShipmentInfoForNotification : IRequest<ShipmentData>
    {
        public GetShipmentInfoForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}