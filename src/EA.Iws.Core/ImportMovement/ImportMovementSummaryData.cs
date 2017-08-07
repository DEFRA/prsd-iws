namespace EA.Iws.Core.ImportMovement
{
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
    }
}
