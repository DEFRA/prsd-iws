namespace EA.Iws.Core.MovementDocument
{
    using System;
    using Shared;

    public class ShipmentData
    {
        public Guid NotificationId { get; set; }

        public int ShipmentNumber { get; set; }

        public int NumberOfPackages { get; set; }

        public DateTime Date { get; set; }

        public ShipmentQuantityUnits Units { get; set; }

        public decimal Quantity { get; set; }
    }
}
