namespace EA.Iws.Core.Movement
{
    using System.Collections.Generic;
    using Shared;

    public class NotificationMovementsSummaryAndTable
    {
        public BasicMovementSummary SummaryData { get; set; }

        public int TotalIntendedShipments { get; set; }

        public NotificationType NotificationType { get; set; }

        public List<MovementTableDataRow> ShipmentTableData { get; set; }
    }
}