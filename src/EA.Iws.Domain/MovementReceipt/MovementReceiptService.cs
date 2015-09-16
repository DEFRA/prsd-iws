namespace EA.Iws.Domain.MovementReceipt
{
    using System;
    using Core.MovementReceipt;
    using Movement;

    public class MovementReceiptService : IMovementReceiptService
    {
        public bool IsReceived(Movement movement)
        {
            return movement.Receipt != null
                && movement.Receipt.Decision == Decision.Accepted
                && movement.Receipt.Quantity.HasValue;
        }
    }
}
