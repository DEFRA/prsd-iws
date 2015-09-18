namespace EA.Iws.Requests.MovementOperationReceipt
{
    using System;
    using Prsd.Core.Mediator;

    public class GetMovementOperationReceiptDataByMovementId : IRequest<MovementOperationReceiptData>
    {
        public Guid Id { get; private set; }

        public GetMovementOperationReceiptDataByMovementId(Guid id)
        {
            Id = id;
        }
    }
}
