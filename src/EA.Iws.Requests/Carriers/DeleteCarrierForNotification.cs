namespace EA.Iws.Requests.Carriers
{
    using System;
    using Prsd.Core.Mediator;

    public class DeleteCarrierForNotification : IRequest<bool>
    {
        public DeleteCarrierForNotification(Guid notificationId, Guid carrierId)
        {
            NotificationId = notificationId;
            CarrierId = carrierId;
        }

        public Guid CarrierId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}