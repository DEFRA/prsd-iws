namespace EA.Iws.Requests.Movement.Edit
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetMaximumValidMovementDate : IRequest<ValidMovementDates>
    {
        public Guid MovementId { get; private set; }

        public GetMaximumValidMovementDate(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}