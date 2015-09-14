namespace EA.Iws.Requests.MovementReceipt
{
    using System;
    using Core.MovementReceipt;
    using Prsd.Core.Mediator;

    public class GetMovementReceiptQuantityByMovementId : IRequest<MovementReceiptQuantityData>
    {
        public Guid Id { get; private set; }

        public GetMovementReceiptQuantityByMovementId(Guid id)
        {
            Id = id;
        }
    }
}
