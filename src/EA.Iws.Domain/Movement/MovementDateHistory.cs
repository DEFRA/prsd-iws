namespace EA.Iws.Domain.Movement
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class MovementDateHistory : Entity
    {
        public Guid MovementId { get; private set; }
        public DateTimeOffset PreviousDate { get; private set; }
        public DateTimeOffset DateChanged { get; private set; }

        protected MovementDateHistory()
        {
        }

        public MovementDateHistory(Guid movementId, DateTimeOffset previousDate)
        {
            Guard.ArgumentNotDefaultValue(() => movementId, movementId);
            Guard.ArgumentNotDefaultValue(() => previousDate, previousDate);

            MovementId = movementId;
            PreviousDate = previousDate;
            DateChanged = new DateTimeOffset(SystemTime.UtcNow, TimeSpan.Zero);
        }
    }
}