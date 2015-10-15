namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Home
{
    using System;
    using Core.Movement;
    using Prsd.Core.Helpers;

    public class MovementSummaryTableViewModel
    {
        public int Number { get; set; }

        public string Status { get; set; }

        public string PreNotification { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public DateTime? Received { get; set; }

        public string Quantity { get; set; }

        public DateTime? RecoveredOrDisposedOf { get; set; }

        public MovementSummaryTableViewModel(MovementSummaryTableData data)
        {
            Number = data.Number;
            Status = EnumHelper.GetDisplayName(data.Status);
            PreNotification = "- -";
            ShipmentDate = data.ShipmentDate;
            Received = data.ReceivedDate;
            Quantity = data.Quantity.HasValue ? data.Quantity.Value.ToString("G29") + " " + EnumHelper.GetDisplayName(data.QuantityUnits.GetValueOrDefault()) : "- -";
            RecoveredOrDisposedOf = data.CompletedDate;
        }
    }
}