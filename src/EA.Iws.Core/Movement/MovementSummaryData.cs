namespace EA.Iws.Core.Movement
{
    using System;
    using System.Collections.Generic;
    using Shared;

    public class MovementSummaryData
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        public int IntendedShipments { get; set; }

        public int UsedShipments { get; set; }

        public decimal IntendedQuantityTotal { get; set; }

        public decimal ReceivedQuantityTotal { get; set; }

        public ShipmentQuantityUnits DisplayUnits { get; set; }

        public int ActiveLoadsPermitted { get; set; }

        public int ActiveLoadsCurrent { get; set; }

        public List<MovementSummaryTableData> ShipmentTableData { get; set; }

    }
}
