namespace EA.Iws.Domain.Movement
{
    using System;
    using Prsd.Core;

    public class ActiveMovement
    {
        public bool IsActive(Movement movement)
        {
            return movement.Date.HasValue && movement.Date < SystemTime.UtcNow;
        }
    }
}
