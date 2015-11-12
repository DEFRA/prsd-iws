namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class SetMovementAccepted : IRequest<Guid>
    {
        public SetMovementAccepted(Guid movementId, Guid fileId, DateTime dateReceived, decimal quantity, ShipmentQuantityUnits units)
        {
            MovementId = movementId;
            FileId = fileId;
            DateReceived = dateReceived;
            Quantity = quantity;
            Units = units;
        }

        public Guid MovementId { get; private set; }
        public Guid FileId { get; private set; }
        public DateTime DateReceived { get; private set; }
        public decimal Quantity { get; private set; }
        public ShipmentQuantityUnits Units { get; private set; }
    }
}
