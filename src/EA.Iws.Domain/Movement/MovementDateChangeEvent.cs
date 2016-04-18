namespace EA.Iws.Domain.Movement
{
    using System;
    using Prsd.Core.Domain;

    public class MovementDateChangeEvent : IEvent
    {
        public Guid MovementId { get; private set; }
        public DateTime PreviousDate { get; private set; }

        public MovementDateChangeEvent(Guid movementId, DateTime previousDate)
        {
            MovementId = movementId;
            PreviousDate = previousDate;
        }
    }
}