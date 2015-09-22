namespace EA.Iws.Domain.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using Prsd.Core;

    public class ActiveMovements
    {
        public int Total(IList<Movement> movements)
        {
            return List(movements).Count();
        }

        public IList<Movement> List(IList<Movement> movements)
        {
            return movements.Where(m => m.IsActive).ToArray();
        }
    }
}
