namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class SetActualDateOfMovement : IRequest<Guid>
    {
        public SetActualDateOfMovement(Guid movementId, DateTime date)
        {
            MovementId = movementId;
            Date = date;
        }

        public Guid MovementId { get; private set; }
        public DateTime Date { get; private set; }
    }
}
