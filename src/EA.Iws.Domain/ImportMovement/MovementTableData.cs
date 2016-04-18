namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using Core.ImportMovement;
    using Core.Shared;

    public class MovementTableData
    {
        public int Number { get; private set; }

        public DateTime? PreNotification { get; private set; }

        public DateTime? ShipmentDate { get; private set; }

        public DateTime? Received { get; private set; }

        public decimal? Quantity { get; private set; }

        public ShipmentQuantityUnits? Unit { get; private set; }

        public DateTime? Rejected { get; private set; }

        public DateTime? RecoveredOrDisposedOf { get; private set; }

        public ImportMovementStatus Status { get; private set; }

        public static MovementTableData Load(ImportMovement movement,
            ImportMovementReceipt movementReceipt,
            ImportMovementRejection movementRejection,
            ImportMovementCompletedReceipt movementOperationReceipt)
        {
            var data = new MovementTableData();

            if (movement != null)
            {
                data.Number = movement.Number;
                data.PreNotification = movement.PrenotificationDate;
                data.ShipmentDate = movement.ActualShipmentDate;
            }

            if (movementReceipt != null)
            {
                data.Received = movementReceipt.Date;
                data.Quantity = movementReceipt.Quantity;
                data.Unit = movementReceipt.Unit;
            }

            if (movementRejection != null)
            {
                data.Rejected = movementRejection.Date;
            }

            if (movementOperationReceipt != null)
            {
                data.RecoveredOrDisposedOf = movementOperationReceipt.Date;
            }

            return data;
        }
    }
}
