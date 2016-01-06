namespace EA.Iws.Requests.Movement.Receive
{
    using System;
    using Core.Movement;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class DoesQuantityReceivedExceedTolerance : IRequest<QuantityReceivedTolerance>
    {
        public Guid MovementId { get; private set; }

        public decimal Quantity { get; private set; }

        public ShipmentQuantityUnits Units { get; private set; }

        public DoesQuantityReceivedExceedTolerance(Guid movementId, decimal quantity, ShipmentQuantityUnits units)
        {
            MovementId = movementId;
            Quantity = quantity;
            Units = units;
        }
    }
}
