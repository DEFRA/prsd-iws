namespace EA.Iws.Core.ImportMovement
{
    public class ImportMovementSummaryData
    {
        public ImportMovementData Data { get; set; }

        public ImportMovementReceiptData ReceiptData { get; set; }

        public ImportMovementRecoveryData RecoveryData { get; set; }

        public string Comments { get; set; }

        public string StatsMarking { get; set; }
    }
}
