namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class GetShipmentUnits : IRequest<ShipmentQuantityUnits>
    {
        public Guid NotificationId { get; private set; }

        public GetShipmentUnits(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}