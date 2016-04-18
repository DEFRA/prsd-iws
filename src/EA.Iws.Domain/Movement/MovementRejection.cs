namespace EA.Iws.Domain.Movement
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class MovementRejection : Entity
    {
        public Guid MovementId { get; private set; }

        public DateTime Date { get; private set; }

        public string Reason { get; private set; }

        public string FurtherDetails { get; private set; }

        public Guid? FileId { get; private set; }

        protected MovementRejection()
        {
        }

        public MovementRejection(Guid movementId,
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

        public void SetFile(Guid fileId)
        {
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);
            FileId = fileId;
        }
    }
}
