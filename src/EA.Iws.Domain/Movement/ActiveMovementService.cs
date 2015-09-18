namespace EA.Iws.Domain.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using Prsd.Core;

    public class ActiveMovementService
    {
        public int TotalActiveMovements(IList<Movement> movements)
        {
            return ActiveMovements(movements).Count();
        }

        public IList<Movement> ActiveMovements(IList<Movement> movements)
        {
            return movements.Where(m => m.IsActive).ToArray();
        }
    }
}
