namespace EA.Iws.Core.ImportMovement
{
    using EA.Iws.Core.Shared;
    using System;
    public class ImportMovementSummaryData
    {
        public ImportMovementData Data { get; set; }

        public ImportMovementReceiptData ReceiptData { get; set; }

        public ImportMovementRecoveryData RecoveryData { get; set; }

        public string Comments { get; set; }

        public string StatsMarking { get; set; }

        public Guid MovementId { get; set; }

        public bool HasNoPrenotification { get; set; }

        public DateTime? RejectionDate { get; set; }

        public bool IsRejected { get; set; }

        public bool IsReceived { get; set; }

        public bool IsPartiallyRejected { get; set; }

        public decimal? RejectedQuantity { get; set; }

        public ShipmentQuantityUnits? RejectedUnit { get; set; }
    }
}
