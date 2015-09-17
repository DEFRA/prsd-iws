namespace EA.Iws.Domain.Movement
{
    using EA.Prsd.Core;
    using System;

    public class ActiveMovement : IActiveMovement
    {
        public bool IsActive(Movement movement)
        {
            return movement.Date.HasValue && movement.Date < SystemTime.UtcNow;
        }
    }
}
