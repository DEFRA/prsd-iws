namespace EA.Iws.Core.Notification.Overview
{
    using System;
    using IntendedShipments;
    using Shared;

    public class ShipmentOverview
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public IntendedShipmentData IntendedShipmentData { get; set; }
    }
}
