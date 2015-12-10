namespace EA.Iws.Requests.ImportMovement.Reject
{
    using System;
    using Prsd.Core.Mediator;

    public class RecordRejection : IRequest<bool>
    {
        public Guid ImportMovementId { get; private set; }

        public DateTime Date { get; private set; }

        public string Reason { get; private set; }

        public string FurtherDetails { get; private set; }

        public RecordRejection(Guid importMovementId, 
            DateTime date, 
            string reason, 
            string furtherDetails)
        {
            ImportMovementId = importMovementId;
            Date = date;
            Reason = reason;
            FurtherDetails = furtherDetails;
        }
    }
}
