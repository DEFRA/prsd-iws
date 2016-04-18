namespace EA.Iws.Core.ImportNotificationMovements
{
    using System;
    using Shared;

    public class Summary
    {
        public string NotificationNumber { get; set; }

        public Guid Id { get; set; }

        public NotificationType NotificationType { get; set; }

        public int IntendedShipments { get; set; }

        public int UsedShipments { get; set; }

        public decimal QuantityRemainingTotal { get; set; }

        public decimal QuantityReceivedTotal { get; set; }

        public ShipmentQuantityUnits DisplayUnit { get; set; }
    }
}
