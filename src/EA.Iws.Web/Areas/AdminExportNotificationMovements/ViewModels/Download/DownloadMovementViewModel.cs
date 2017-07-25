namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Download
{
    using System;
    using System.ComponentModel;
    using Core.Movement;
    using Core.Shared;
    using Prsd.Core.Helpers;

    public class DownloadMovementViewModel
    {
        public DownloadMovementViewModel(MovementTableDataRow data, NotificationType type, string notificationNumber)
        {
            Number = data.Number;
            Status = GetStatusDisplay(data.Status, type);
            SubmittedDate = data.SubmittedDate;
            ShipmentDate = data.ShipmentDate;
            ReceivedDate = data.ReceivedDate;
            Quantity = data.Quantity;
            Unit = data.QuantityUnits.HasValue ? EnumHelper.GetDisplayName(data.QuantityUnits) : null;
            CompletedDate = data.CompletedDate;
            NotificationNumber = notificationNumber.Replace(" ", string.Empty);
        }

        [DisplayName("Notification number")]
        public string NotificationNumber { get; set; }

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

        private static string GetStatusDisplay(MovementStatus status, NotificationType type)
        {
            if (status == MovementStatus.Completed)
            {
                return type == NotificationType.Disposal ? "Disposed" : "Recovered";
            }

            return EnumHelper.GetDisplayName(status);
        }
    }
}