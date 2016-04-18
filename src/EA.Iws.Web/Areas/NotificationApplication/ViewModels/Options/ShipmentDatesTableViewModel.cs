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

        public DateTime? Received { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public DateTime? RecoveredOrDisposedOf { get; set; }
        
        public ShipmentDatesTableViewModel(MovementTableDataRow data)
        {
            Number = data.Number;
            Status = data.Status;
            PreNotification = data.SubmittedDate;
            ShipmentDate = data.ShipmentDate;
            HasShipped = data.HasShipped;
            Received = data.ReceivedDate;
            Quantity = data.Quantity;
            Unit = data.QuantityUnits;
            RecoveredOrDisposedOf = data.CompletedDate;
        }
    }
}