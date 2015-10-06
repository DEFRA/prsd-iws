namespace EA.Iws.Requests.Notification.Overview
{
    using System;
    using Core.IntendedShipments;
    using Core.Shared;

    public class ShipmentOverview
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public IntendedShipmentData IntendedShipmentData { get; set; }
    }
}
