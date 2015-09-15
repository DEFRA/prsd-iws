namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNotificationIdByMovementId : IRequest<Guid>
    {
        public Guid MovementId { get; private set; }

        public GetNotificationIdByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
