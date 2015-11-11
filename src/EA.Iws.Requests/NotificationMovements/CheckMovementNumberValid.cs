namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Prsd.Core.Mediator;

    public class CheckMovementNumberValid : IRequest<MovementNumberStatus>
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        public CheckMovementNumberValid(Guid notificationId, int number)
        {
            NotificationId = notificationId;
            Number = number;
        }
    }
}
