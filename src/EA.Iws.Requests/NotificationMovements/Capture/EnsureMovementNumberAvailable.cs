namespace EA.Iws.Requests.NotificationMovements.Capture
{
    using System;
    using Prsd.Core.Mediator;

    public class EnsureMovementNumberAvailable : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }
        public int Number { get; private set; }

        public EnsureMovementNumberAvailable(Guid notificationId, int number)
        {
            NotificationId = notificationId;
            Number = number;
        }
    }
}
