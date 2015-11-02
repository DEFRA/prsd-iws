namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Home
{
    using System;
    using System.Linq;
    using Core.Movement;
    using Prsd.Core.Helpers;

    public class MovementSummaryTableViewModel
    {
        private static readonly string[] HideQuantityStatus = new[] { "New", "Cancelled" };

        public int Number { get; set; }

        public string Status { get; set; }

        public DateTime? PreNotification { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public DateTime? Received { get; set; }

        public string Quantity { get; set; }

        public DateTime? RecoveredOrDisposedOf { get; set; }

        public MovementSummaryTableViewModel(MovementSummaryTableData data)
        {
            Number = data.Number;
            Status = EnumHelper.GetDisplayName(data.Status);
            PreNotification = data.SubmittedDate;
            ShipmentDate = data.ShipmentDate;
            Received = data.ReceivedDate;
            Quantity = data.Quantity.HasValue && !HideQuantityStatus.Contains(Status) 
                ? data.Quantity.Value.ToString("G29") + " " + EnumHelper.GetDisplayName(data.QuantityUnits.GetValueOrDefault()) 
                : "- -";
            RecoveredOrDisposedOf = data.CompletedDate;
        }
    }
}