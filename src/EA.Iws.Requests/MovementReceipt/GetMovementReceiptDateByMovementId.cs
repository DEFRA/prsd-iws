namespace EA.Iws.Requests.MovementReceipt
{
    using System;
    using EA.Prsd.Core.Mediator;

    public class GetMovementReceiptDateByMovementId : IRequest<DateTime?>
    {
        public Guid MovementId { get; private set; }

        public GetMovementReceiptDateByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
