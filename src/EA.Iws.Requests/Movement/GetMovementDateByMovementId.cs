namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class GetMovementDateByMovementId : IRequest<DateTime>
    {
        public Guid MovementId { get; private set; }

        public GetMovementDateByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
