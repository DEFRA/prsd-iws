namespace EA.Iws.Core.Movement
{
    using System;
    using Shared;

    public class MovementSummaryTableData
    {
        public int Number { get; set; }

        public string Status { get; set; }

        public DateTime? PreNotification { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public DateTime? Received { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? QuantityUnits { get; set; }

        public DateTime? RecoveredOrDisposedOf { get; set; }
    }
}
