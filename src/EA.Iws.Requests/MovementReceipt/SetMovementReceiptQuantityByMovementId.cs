namespace EA.Iws.Requests.MovementReceipt
{
    using System;
    using Prsd.Core.Mediator;

    public class SetMovementReceiptQuantityByMovementId : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public decimal Quantity { get; private set; }

        public SetMovementReceiptQuantityByMovementId(Guid id, decimal quantity)
        {
            Id = id;
            Quantity = quantity;
        }
    }
}
