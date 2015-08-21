namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.IntendedShipments;
    using Core.Notification;
    using Core.Shared;

    public class AmountsAndDatesInfo
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsIntendedShipmentsCompleted { get; set; }
        public IntendedShipmentData IntendedShipmentData { get; set; }
    }
}
