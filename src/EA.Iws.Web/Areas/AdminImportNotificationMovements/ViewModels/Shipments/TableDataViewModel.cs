namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Shipments
{
    using System;
    using Core.ImportMovement;
    using Core.ImportNotificationMovements;
    using Core.Shared;

    public class TableDataViewModel
    {
        public TableDataViewModel(MovementTableData data)
        {
            Number = data.Number;
            PreNotification = data.PreNotification;
            ShipmentDate = data.ShipmentDate;
            Received = data.Received;
            Quantity = data.Quantity;
            Unit = data.Unit;
            Rejected = data.Rejected;
            RecoveredOrDisposedOf = data.RecoveredOrDisposedOf;
            Status = data.Status;
        }

        public int Number { get; set; }

        public DateTime? PreNotification { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public bool HasShipped { get; set; }

        public DateTime? Received { get; set; }

        public DateTime? Rejected { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public DateTime? RecoveredOrDisposedOf { get; set; }

        public ImportMovementStatus Status { get; set; }
    }
}