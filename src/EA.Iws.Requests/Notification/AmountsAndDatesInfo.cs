namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification;
    using Core.Shared;
    using Core.Shipment;

    public class AmountsAndDatesInfo
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsIntendedShipmentsCompleted { get; set; }
        public ShipmentData ShipmentData { get; set; }
    }
}
