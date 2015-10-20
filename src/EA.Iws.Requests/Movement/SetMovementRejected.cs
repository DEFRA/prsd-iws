namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class SetMovementRejected : IRequest<Guid>
    {
        public SetMovementRejected(Guid movementId, Guid fileId, DateTime dateReceived, string reason)
        {
            MovementId = movementId;
            FileId = fileId;
            DateReceived = dateReceived;
            Reason = reason;
        }

        public Guid MovementId { get; private set; }
        public Guid FileId { get; private set; }
        public DateTime DateReceived { get; private set; }
        public string Reason { get; private set; }
    }
}
