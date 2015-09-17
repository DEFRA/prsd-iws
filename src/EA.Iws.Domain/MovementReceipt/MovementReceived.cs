namespace EA.Iws.Domain.MovementReceipt
{
    using System;
    using Core.MovementReceipt;
    using Movement;

    public class MovementReceived
    {
        public bool IsReceived(Movement movement)
        {
            return movement.Receipt != null
                && movement.Receipt.Decision == Decision.Accepted
                && movement.Receipt.Quantity.HasValue;
        }
    }
}
