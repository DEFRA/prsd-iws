namespace EA.Iws.Core.Movement
{
    using System;

    public class MovementReceiptSummaryData
    {
        public Guid NotificationId { get; set; }

        public Guid MovementId { get; set; }

        public string NotificationNumber { get; set; }

        public int ThisMovementNumber { get; set; }

        public int ActiveLoadsPermitted { get; set; }

        public int CurrentActiveLoads { get; set; }

        public decimal QuantitySoFar { get; set; }

        public decimal QuantityRemaining { get; set; }

        public Core.Shared.ShipmentQuantityUnits DisplayUnit { get; set; }
    }
}
