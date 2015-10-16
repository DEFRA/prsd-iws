namespace EA.Iws.Requests.MovementOperationReceipt
{
    using Prsd.Core.Mediator;
    using System;

    public class CreateMovementOperationReceiptForMovement : IRequest<bool>
    {
        public Guid MovementId { get; private set; }
        public DateTime DateComplete { get; private set; }

        public CreateMovementOperationReceiptForMovement(Guid movementId, DateTime dateComplete)
        {
            MovementId = movementId;
            DateComplete = dateComplete;
        }
    }
}
