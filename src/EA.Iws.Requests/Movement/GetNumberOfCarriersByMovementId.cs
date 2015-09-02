namespace EA.Iws.Requests.Movement
{
    using Prsd.Core.Mediator;
    using System;

    public class GetNumberOfCarriersByMovementId : IRequest<int?>
    {
        public Guid MovementId { get; private set; }

        public GetNumberOfCarriersByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
