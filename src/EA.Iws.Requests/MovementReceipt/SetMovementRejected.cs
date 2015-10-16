namespace EA.Iws.Requests.MovementReceipt
{
    using System;
    using Prsd.Core.Mediator;

    public class SetMovementRejected : IRequest<Guid>
    {
        public SetMovementRejected(Guid movementId, Guid fileId, DateTime dateReceived, string reason)
        {
            MovementId = movementId;
            FileId = fileId;
            DateReceied = dateReceived;
            Reason = reason;
        }

        public Guid MovementId { get; private set; }
        public Guid FileId { get; private set; }
        public DateTime DateReceied { get; private set; }
        public string Reason { get; private set; }
    }
}
