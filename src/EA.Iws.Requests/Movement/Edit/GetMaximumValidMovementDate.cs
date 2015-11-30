namespace EA.Iws.Requests.Movement.Edit
{
    using System;
    using Prsd.Core.Mediator;

    public class GetMaximumValidMovementDate : IRequest<DateTime>
    {
        public Guid MovementId { get; private set; }

        public GetMaximumValidMovementDate(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}