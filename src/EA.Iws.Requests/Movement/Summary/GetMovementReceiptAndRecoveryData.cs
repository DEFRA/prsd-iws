namespace EA.Iws.Requests.Movement.Summary
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetMovementReceiptAndRecoveryData : IRequest<MovementReceiptAndRecoveryData>
    {
        public Guid MovementId { get; private set; }

        public GetMovementReceiptAndRecoveryData(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
