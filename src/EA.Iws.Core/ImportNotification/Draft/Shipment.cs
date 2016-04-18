namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.ComponentModel;
    using Shared;

    [DisplayName("Intended shipments")]
    public class Shipment : IDraftEntity
    {
        internal Shipment()
        {
        }

        public Shipment(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        public Guid ImportNotificationId { get; private set; }

        public int? TotalShipments { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}