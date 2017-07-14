namespace EA.Iws.Requests.ImportMovement
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNotificationIdByMovementId : IRequest<Guid>
    {
        public GetNotificationIdByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }

        public Guid MovementId { get; private set; }
    }
}