namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class SetMovementQuantityByMovementId : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public decimal Quantity { get; private set; }

        public ShipmentQuantityUnits Units { get; private set; }

        public SetMovementQuantityByMovementId(Guid id, decimal quantity, ShipmentQuantityUnits units)
        {
            Id = id;
            Quantity = quantity;
            Units = units;
        }
    }
}
