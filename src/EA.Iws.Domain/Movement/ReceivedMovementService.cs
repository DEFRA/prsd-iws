namespace EA.Iws.Domain.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.MovementReceipt;

    public class ReceivedMovementService
    {
        public IList<Movement> ReceivedMovements(IList<Movement> movements)
        {
            return movements.Where(m => m.IsReceived).ToArray();
        }
    }
}
