namespace EA.Iws.Domain.Movement
{
    using System;
    using EA.Iws.Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class MovementRejection : Entity
    {
        public Guid MovementId { get; private set; }

        public DateTime Date { get; private set; }

        public string Reason { get; private set; }

        public Guid? FileId { get; private set; }

        public decimal? RejectedQuantity { get; set; }

        public ShipmentQuantityUnits? RejectedUnit { get; set; }

        protected MovementRejection()
        {
        }

        public MovementRejection(Guid movementId,
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

        public void SetFile(Guid fileId)
        {
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);
            FileId = fileId;
        }
    }
}
