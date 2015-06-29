namespace EA.Iws.Requests.TransitState
{
    using System;
    using Prsd.Core.Mediator;

    public class RemoveTransitStateForNotification : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public Guid TransitStateId { get; private set; }

        public RemoveTransitStateForNotification(Guid notificationId, Guid transitStateId)
        {
            NotificationId = notificationId;
            TransitStateId = transitStateId;
        }
    }
}
