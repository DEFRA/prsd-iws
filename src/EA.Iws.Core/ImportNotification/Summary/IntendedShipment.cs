namespace EA.Iws.Core.ImportNotification.Summary
{
    using System;
    using Shared;

    public class IntendedShipment
    {
        public ShipmentQuantityUnits? Units { get; set; }

        public int? TotalShipments { get; set; }

        public decimal? Quantity { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }
    }
}
