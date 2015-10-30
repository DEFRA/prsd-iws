namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using Shared;

    public class Shipment
    {
        public Guid ImportNotificationId { get; private set; }
        public int? TotalShipments { get; set; }
        public decimal? Quantity { get; set; }
        public ShipmentQuantityUnits? Unit { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Shipment(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}