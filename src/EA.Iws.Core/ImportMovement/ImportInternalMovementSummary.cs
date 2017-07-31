namespace EA.Iws.Core.ImportMovement
{
    using Core.Shared;
    using ImportNotificationMovements;

    public class ImportInternalMovementSummary
    {
        public Summary SummaryData { get; set; }

        public decimal AverageTonnage { get; set; }

        public ShipmentQuantityUnits AverageDataUnit { get; set; }
    }
}
