namespace EA.Iws.Core.Shipment
{
    using System;

    public class NumberOfShipmentsHistoryData
    {
        public Guid NotificaitonId { get; set; }

        public int NumberOfShipments { get; set; }

        public DateTime DateChanged { get; set; }

        public bool HasHistoryData { get; set; }
    }
}
