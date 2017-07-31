namespace EA.Iws.Core.Movement
{
    using Core.Shared;
    public class InternalMovementSummary
    {
        public BasicMovementSummary SummaryData { get; set; }

         public int TotalIntendedShipments { get; set; }
        public decimal AverageTonnage { get; set; }

        public ShipmentQuantityUnits AverageDataUnit { get; set; }
    }
}
