namespace EA.Iws.Domain.Movement
{
    using System.Collections.Generic;
    using System.Linq;

    public class ReceivedMovements
    {
        private readonly ActiveMovements activeMovements;

        public ReceivedMovements(ActiveMovements activeMovements)
        {
            this.activeMovements = activeMovements;
        }

        public IList<Movement> List(IList<Movement> movements)
        {
            return movements.Where(m => m.IsReceived).ToArray();
        }

        public IList<Movement> ListActive(IList<Movement> movements)
        {
            return List(activeMovements.List(movements));
        }
    }
}
