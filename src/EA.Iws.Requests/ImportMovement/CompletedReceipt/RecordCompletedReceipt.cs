namespace EA.Iws.Requests.ImportMovement.CompletedReceipt
{
    using System;
    using Prsd.Core.Mediator;

    public class RecordCompletedReceipt : IRequest<bool>
    {
        public Guid ImportMovementId { get; private set; }

        public DateTime Date { get; private set; }

        public RecordCompletedReceipt(Guid importMovementId, DateTime date)
        {
            ImportMovementId = importMovementId;
            Date = date;
        }
    }
}
