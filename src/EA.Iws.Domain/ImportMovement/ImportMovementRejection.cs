namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ImportMovementRejection : Entity
    {
        public Guid MovementId { get; private set; }

        public DateTime Date { get; private set; }

        public string Reason { get; private set; }

        public string FurtherDetails { get; private set; }

        protected ImportMovementRejection()
        {
        }

        public ImportMovementRejection(Guid movementId,
            DateTime date, 
            string reason, 
            string furtherDetails)
        {
            Guard.ArgumentNotNullOrEmpty(() => reason, reason);

            MovementId = movementId;
            Date = date;
            Reason = reason;
            FurtherDetails = furtherDetails;
        }
    }
}
