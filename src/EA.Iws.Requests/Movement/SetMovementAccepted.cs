namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class SetMovementAccepted : IRequest<Guid>
    {
        public SetMovementAccepted(Guid movementId, Guid fileId, DateTime dateReceived, decimal quantity)
        {
            MovementId = movementId;
            FileId = fileId;
            DateReceived = dateReceived;
            Quantity = quantity;
        }

        public Guid MovementId { get; private set; }
        public Guid FileId { get; private set; }
        public DateTime DateReceived { get; private set; }
        public decimal Quantity { get; private set; }
    }
}
