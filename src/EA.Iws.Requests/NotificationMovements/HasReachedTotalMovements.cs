namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Prsd.Core.Mediator;

    public class HasReachedTotalMovements : IRequest<bool>
    {
        public HasReachedTotalMovements(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}