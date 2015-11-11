namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class GetMovementIdByNumber : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        public GetMovementIdByNumber(Guid notificationId, int number)
        {
            NotificationId = notificationId;
            Number = number;
        }
    }
}
