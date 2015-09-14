namespace EA.Iws.Requests.MovementReceipt
{
    using System;
    using Prsd.Core.Mediator;

    public class GetMovementAcceptanceDataByMovementId : IRequest<MovementAcceptanceData>
    {
        public Guid MovementId { get; private set; }

        public GetMovementAcceptanceDataByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
