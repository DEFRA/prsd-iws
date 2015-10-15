namespace EA.Iws.Domain.Movement
{
    using Core.Movement;
    using Prsd.Core.Domain;

    public class MovementStatusChangeEvent : IEvent
    {
        public Movement Movement { get; private set; }
        public MovementStatus TargetStatus { get; private set; }

        public MovementStatusChangeEvent(Movement movement, MovementStatus targetStatus)
        {
            TargetStatus = targetStatus;
            Movement = movement;
        }
    }
}
