namespace EA.Iws.Domain.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.MovementReceipt;

    public class ReceivedMovements
    {
        public IList<Movement> List(IList<Movement> movements)
        {
            return movements.Where(m => m.IsReceived).ToArray();
        }
    }
}
