namespace EA.Iws.Core.Movement.Bulk
{
    using System;
    using System.Collections.Generic;
    using PackagingType;
    using Shared;

    [Serializable]
    public class PrenotificationMovement
    {
        public PrenotificationMovement()
        {
        }

        public string NotificationNumber { get; set; }

        public int? ShipmentNumber { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public IList<PackagingType> PackagingTypes { get; set; }

        public DateTime? ActualDateOfShipment { get; set; }
    }
}
