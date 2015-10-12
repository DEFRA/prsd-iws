namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Home
{
    using Core.Movement;
    using Prsd.Core.Helpers;

    public class MovementSummaryTableViewModel
    {
        public int Number { get; set; }

        public string Status { get; set; }

        public string PreNotification { get; set; }

        public string ShipmentDate { get; set; }

        public string Received { get; set; }

        public string Quantity { get; set; }

        public string RecoveredOrDisposedOf { get; set; }

        public MovementSummaryTableViewModel(MovementSummaryTableData data)
        {
            Number = data.Number;
            Status = "Indetermination";
            PreNotification = "- -";
            ShipmentDate = data.ShipmentDate != null ? data.ShipmentDate.GetValueOrDefault().ToString("dd-MM-yyyy") : "- -";
            Received = data.Received != null ? data.Received.GetValueOrDefault().ToString("dd-MM-yyyy") : "- -";
            Quantity = data.Quantity.HasValue ? data.Quantity.Value.ToString("G29") + " " + EnumHelper.GetDisplayName(data.QuantityUnits.GetValueOrDefault()) : "- -";
            RecoveredOrDisposedOf = data.RecoveredOrDisposedOf != null ? data.RecoveredOrDisposedOf.GetValueOrDefault().ToString("dd-MM-yyyy") : "- -";
        }
    }
}