namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using EA.Iws.Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ImportMovementRejection : Entity
    {
        public Guid MovementId { get; private set; }

        public DateTime Date { get; private set; }

        public string Reason { get; private set; }

        public decimal? RejectedQuantity { get; set; }

        public ShipmentQuantityUnits? RejectedUnit { get; set; }

        protected ImportMovementRejection()
        {
        }

        public ImportMovementRejection(Guid movementId,
            DateTime date, 
            string reason,
            decimal? quantity,
            ShipmentQuantityUnits? unit)
        {
            Guard.ArgumentNotNullOrEmpty(() => reason, reason);

            MovementId = movementId;
            Date = date;
            Reason = reason;
            RejectedQuantity = quantity;
            RejectedUnit = unit;
        }
    }
}
