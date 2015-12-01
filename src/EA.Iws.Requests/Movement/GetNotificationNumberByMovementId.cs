namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNotificationNumberByMovementId : IRequest<string>
    {
        public Guid MovementId { get; private set; }

        public GetNotificationNumberByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}