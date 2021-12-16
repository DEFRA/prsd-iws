namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using EA.Iws.Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ImportMovementPartialRejection : Entity
    {
        public Guid MovementId { get; private set; }

        public decimal ActualQuantity { get; set; }

        public ShipmentQuantityUnits ActualUnit { get; set; }

        public decimal RejectedQuantity { get; set; }

        public ShipmentQuantityUnits RejectedUnit { get; set; }

        public DateTime WasteReceivedDate { get; private set; }

        public string Reason { get; private set; }

        public Guid? FileId { get; private set; }

        public DateTime? WasteDisposedDate { get; set; }

        protected ImportMovementPartialRejection()
        {
        }

        public ImportMovementPartialRejection(Guid movementId,
            DateTime wasteReceivedDate,
            string reason,
            decimal actualQuantity,
            ShipmentQuantityUnits actualUnit,
            decimal rejectedQuantity,
            ShipmentQuantityUnits rejectedUnit,
            DateTime? wasteDisposedDate)
        {
            Guard.ArgumentNotNullOrEmpty(() => reason, reason);

            MovementId = movementId;
            WasteReceivedDate = wasteReceivedDate;
            Reason = reason;
            ActualQuantity = actualQuantity;
            ActualUnit = actualUnit;
            RejectedQuantity = rejectedQuantity;
            RejectedUnit = rejectedUnit;
            WasteDisposedDate = wasteDisposedDate;
        }

        public void SetFile(Guid fileId)
        {
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);
            FileId = fileId;
        }
    }
}