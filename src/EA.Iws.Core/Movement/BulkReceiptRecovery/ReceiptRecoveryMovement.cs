namespace EA.Iws.Core.Movement.BulkReceiptRecovery
{
    using System;
    using Shared;

    public class ReceiptRecoveryMovement
    {
        public ReceiptRecoveryMovement()
        {
        }

        public string NotificationNumber { get; set; }

        public int? ShipmentNumber { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public DateTime? RecoveredDisposedDate { get; set; }

        public bool MissingNotificationNumber { get; set; }

        public bool MissingShipmentNumber { get; set; }

        public bool MissingReceivedDate { get; set; }

        public bool MissingQuantity { get; set; }

        public bool MissingUnits { get; set; }

        public bool MissingRecoveredDisposedDate { get; set; }
    }
}
