namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Home
{
    using System;
    using Core.Movement;
    using Core.Shared;
    using Prsd.Core;

    public class MovementSummaryTableViewModel
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public MovementStatus Status { get; set; }

        public DateTime? PreNotification { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public DateTime? Received { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public DateTime? RecoveredOrDisposedOf { get; set; }

        public ShipmentType? WasShipmentAccepted { get; set; }

        public MovementSummaryTableViewModel(MovementTableDataRow data)
        {
            Id = data.Id;
            Number = data.Number;
            Status = data.Status;
            PreNotification = data.SubmittedDate;
            ShipmentDate = data.ShipmentDate;
            Received = data.ReceivedDate;
            Quantity = data.Quantity;
            Unit = data.QuantityUnits;
            RecoveredOrDisposedOf = data.CompletedDate;
            WasShipmentAccepted = GetShipmentOutcome(data);
        }

        public bool IsShipped()
        {
            return Status == MovementStatus.Submitted && ShipmentDate < SystemTime.UtcNow;
        }

        public bool IsShipmentActive()
        {
            return (Status == MovementStatus.New || Status == MovementStatus.Captured) && ShipmentDate <= SystemTime.UtcNow;
        }

        // Determine the shipment outcome strictly from what is recorded on the row.
        // Previously this used a ternary that fell through to ShipmentType.Rejected whenever
        // IsReceived and IsPartialRejection were both false, which caused newly captured /
        // prenotified shipments (no outcome yet) to incorrectly display as "Rejected".
        // Returning null lets the view render "- -" for the no-outcome case.
        private static ShipmentType? GetShipmentOutcome(MovementTableDataRow data)
        {
            if (data.IsReceived)
            {
                return ShipmentType.Accepted;
            }

            if (data.IsPartialRejection)
            {
                return ShipmentType.Partially;
            }

            if (data.Status == MovementStatus.Rejected)
            {
                return ShipmentType.Rejected;
            }

            return null;
        }
    }
}