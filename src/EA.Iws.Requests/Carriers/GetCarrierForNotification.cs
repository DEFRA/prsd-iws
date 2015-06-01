namespace EA.Iws.Requests.Carriers
{
    using System;
    using Prsd.Core.Mediator;

    public class GetCarrierForNotification : IRequest<CarrierData>
    {
        public GetCarrierForNotification(Guid notificationId, Guid carrierId)
        {
            CarrierId = carrierId;
            NotificationId = notificationId;
        }

        public Guid CarrierId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}