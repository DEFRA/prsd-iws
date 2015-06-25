namespace EA.Iws.Core.Shipment
{
    using System;

    public class ShipmentData
    {
        public Guid NotificationId { get; set; }

        public bool HasShipmentData { get; set; }

        public bool IsPreconsentedRecoveryFacility { get; set; }

        public int NumberOfShipments { get; set; }

        public DateTime FirstDate { get; set; }

        public DateTime LastDate { get; set; }

        public ShipmentQuantityUnits Units { get; set; }

        public decimal Quantity { get; set; }
    }
}