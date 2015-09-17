namespace EA.Iws.Domain.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using Prsd.Core;

    public class ActiveMovementCalculator
    {
        public int TotalActiveMovements(IList<Movement> movements)
        {
            return ActiveMovements(movements).Count();
        }

        public IList<Movement> ActiveMovements(IList<Movement> movements)
        {
            return movements.Where(IsActive).ToArray();
        }

        public bool IsActive(Movement movement)
        {
            return movement.Date.HasValue 
                && movement.Date < SystemTime.UtcNow;
        }
    }
}
