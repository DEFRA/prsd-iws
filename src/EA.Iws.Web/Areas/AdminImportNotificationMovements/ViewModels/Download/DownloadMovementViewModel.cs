namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Download
{
    using System;
    using System.ComponentModel;
    using Core.ImportNotificationMovements;
    using Core.Shared;
    using Prsd.Core.Helpers;

    public class DownloadMovementViewModel
    {
        public DownloadMovementViewModel(MovementTableData data, NotificationType type)
        {
            Number = data.Number;
            Status = GetStatusDisplay(data, type);
            SubmittedDate = data.PreNotification;
            ShipmentDate = data.ShipmentDate;
            ReceivedDate = data.Received;
            Quantity = data.Quantity;
            Unit = data.Quantity.HasValue ? EnumHelper.GetDisplayName(data.Unit) : null;
            CompletedDate = data.RecoveredOrDisposedOf;
        }

        [DisplayName("Shipment number")]
        public int Number { get; set; }

        public string Status { get; set; }

        [DisplayName("Prenotified")]
        public DateTime? SubmittedDate { get; set; }

        [DisplayName("Shipment due")]
        public DateTime? ShipmentDate { get; set; }

        [DisplayName("Received")]
        public DateTime? ReceivedDate { get; set; }

        public decimal? Quantity { get; set; }

        public string Unit { get; set; }

        [DisplayName("Recovered/Disposed")]
        public DateTime? CompletedDate { get; set; }

        private static string GetStatusDisplay(MovementTableData data, NotificationType type)
        {
            if (data.IsCancelled)
            {
                return "Cancelled";
            }

            if (data.RecoveredOrDisposedOf.HasValue)
            {
                return type == NotificationType.Disposal ? "Disposed" : "Recovered";
            }

            if (data.Received.HasValue)
            {
                return "Received";
            }

            return "Pending";
        }
    }
}