namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class GetMovementQuantityDataByMovementId : IRequest<MovementQuantityData>
    {
        public Guid MovementId { get; private set; }

        public GetMovementQuantityDataByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
