namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using Prsd.Core.Domain;

    public class ImportMovementCompletedReceipt : Entity
    {
        public DateTimeOffset Date { get; private set; }

        public Guid MovementId { get; private set; }

        protected ImportMovementCompletedReceipt()
        {
        }

        internal ImportMovementCompletedReceipt(Guid movementId, DateTime dateComplete)
        {
            Date = dateComplete;
            MovementId = movementId;
        }
    }
}