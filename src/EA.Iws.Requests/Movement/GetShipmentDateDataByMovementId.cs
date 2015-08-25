namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class GetShipmentDateDataByMovementId : IRequest<MovementDatesData>
    {
        public Guid MovementId { get; private set; }

        public GetShipmentDateDataByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
