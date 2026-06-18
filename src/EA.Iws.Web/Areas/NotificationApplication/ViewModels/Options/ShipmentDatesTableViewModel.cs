namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Options
{
    using System;
    using Core.Movement;
    using Core.Shared;

    public class ShipmentDatesTableViewModel
    {
        public int Number { get; set; }

        public MovementStatus Status { get; set; }

        public DateTime? PreNotification { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public bool HasShipped { get; set; }

        public bool IsShipmentActive { get; set; }

        public DateTime? Received { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public DateTime? RecoveredOrDisposedOf { get; set; }

        public ShipmentType? WasShipmentAccepted { get; set; }

        public ShipmentDatesTableViewModel(MovementTableDataRow data)
        {
            Number = data.Number;
            Status = data.Status;
            PreNotification = data.SubmittedDate;
            ShipmentDate = data.ShipmentDate;
            HasShipped = data.HasShipped;
            IsShipmentActive = data.IsShipmentActive;
            Received = data.ReceivedDate;
            Quantity = data.Quantity;
            Unit = data.QuantityUnits;
            RecoveredOrDisposedOf = data.CompletedDate;
            WasShipmentAccepted = GetShipmentOutcome(data);
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