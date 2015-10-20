namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetMovementReceiptSummaryDataByMovementId : IRequest<MovementReceiptSummaryData>
    {
        public Guid Id { get; private set; }

        public GetMovementReceiptSummaryDataByMovementId(Guid id)
        {
            Id = id;
        }
    }
}
