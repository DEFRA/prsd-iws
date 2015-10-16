namespace EA.Iws.Requests.MovementReceipt
{
    using System;
    using Prsd.Core.Mediator;

    public class CreateMovementReceiptForMovement : IRequest<bool>
    {
        public Guid MovementId { get; private set; }
        public DateTime DateReceived { get; private set; }

        public CreateMovementReceiptForMovement(Guid movementId, DateTime dateReceived)
        {
            MovementId = movementId;
            DateReceived = dateReceived;
        }
    }
}
