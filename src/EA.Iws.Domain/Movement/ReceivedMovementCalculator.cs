namespace EA.Iws.Domain.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.MovementReceipt;

    public class ReceivedMovementCalculator
    {
        public IList<Movement> ReceivedMovements(IList<Movement> movements)
        {
            return movements.Where(IsReceived).ToArray();
        }

        public bool IsReceived(Movement movement)
        {
            return movement.Receipt != null
                && movement.Receipt.Decision == Decision.Accepted
                && movement.Receipt.Quantity.HasValue;
        }
    }
}
