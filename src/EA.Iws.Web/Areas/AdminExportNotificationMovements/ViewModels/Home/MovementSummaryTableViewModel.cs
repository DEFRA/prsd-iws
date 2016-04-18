namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Home
{
    using System;
    using Core.Movement;
    using Core.Shared;

    public class MovementSummaryTableViewModel
    {
        public int Number { get; set; }

        public MovementStatus Status { get; set; }

        public DateTime? PreNotification { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public DateTime? Received { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public DateTime? RecoveredOrDisposedOf { get; set; }

        public MovementSummaryTableViewModel(MovementTableDataRow data)
        {
            Number = data.Number;
            Status = data.Status;
            PreNotification = data.SubmittedDate;
            ShipmentDate = data.ShipmentDate;
            Received = data.ReceivedDate;
            Quantity = data.Quantity;
            Unit = data.QuantityUnits;
            RecoveredOrDisposedOf = data.CompletedDate;
        }

        public bool IsShipped()
        {
            return Status == MovementStatus.Submitted && ShipmentDate < DateTime.UtcNow;
        }
    }
}