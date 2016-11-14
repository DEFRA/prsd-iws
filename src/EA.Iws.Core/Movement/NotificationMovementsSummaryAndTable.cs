namespace EA.Iws.Core.Movement
{
    using System.Collections.Generic;
    using FinancialGuarantee;
    using Shared;

    public class NotificationMovementsSummaryAndTable
    {
        public BasicMovementSummary SummaryData { get; set; }

        public int TotalIntendedShipments { get; set; }

        public NotificationType NotificationType { get; set; }

        public List<MovementTableDataRow> ShipmentTableData { get; set; }

        public bool IsInterimNotification { get; set; }

        public FinancialGuaranteeStatus FgStatus { get; set; }
    }
}