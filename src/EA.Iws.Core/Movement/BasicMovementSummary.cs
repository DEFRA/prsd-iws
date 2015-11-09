namespace EA.Iws.Core.Movement
{
    using System;
    using Shared;

    public class BasicMovementSummary
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public int TotalShipments { get; set; }

        public int ActiveLoadsPermitted { get; set; }

        public int CurrentActiveLoads { get; set; }

        public decimal QuantityReceived { get; set; }

        public decimal QuantityRemaining { get; set; }

        public ShipmentQuantityUnits DisplayUnit { get; set; }
    }
}
