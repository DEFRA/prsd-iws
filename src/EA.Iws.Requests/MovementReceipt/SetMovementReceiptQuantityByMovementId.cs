namespace EA.Iws.Requests.MovementReceipt
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class SetMovementReceiptQuantityByMovementId : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public ShipmentQuantityUnits Units { get; private set; }

        public decimal Quantity { get; private set; }

        public SetMovementReceiptQuantityByMovementId(Guid id, ShipmentQuantityUnits units, decimal quantity)
        {
            Id = id;
            Units = units;
            Quantity = quantity;
        }
    }
}
