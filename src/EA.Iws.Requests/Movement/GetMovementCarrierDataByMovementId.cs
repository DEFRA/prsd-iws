namespace EA.Iws.Requests.Movement
{
    using Prsd.Core.Mediator;
    using System;

    public class GetMovementCarrierDataByMovementId : IRequest<MovementCarrierData>
    {
        public Guid MovementId { get; private set; }

        public GetMovementCarrierDataByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
