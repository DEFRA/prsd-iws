namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetShipmentDates : IRequest<ShipmentDates>
    {
        public Guid NotificationId { get; private set; }

        public GetShipmentDates(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}