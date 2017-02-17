namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Download
{
    using System;
    using System.ComponentModel;
    using Core.Movement;
    using Core.Shared;
    using Prsd.Core.Helpers;

    public class DownloadMovementViewModel
    {
        public DownloadMovementViewModel(MovementTableDataRow data, NotificationType type)
        {
            Number = data.Number.ToString();
            Status = GetStatusDisplay(data.Status, type);
            SubmittedDate = DateValue(data.SubmittedDate, data.Status);
            ShipmentDate = DateValue(data.ShipmentDate, data.Status);
            ReceivedDate = DateValue(data.ReceivedDate, data.Status);
            Quantity = data.Quantity.HasValue
                ? data.Quantity.Value.ToString("G29") + " " +
                  EnumHelper.GetShortName(data.QuantityUnits.GetValueOrDefault())
                : "- -";
            CompletedDate = DateValue(data.CompletedDate, data.Status);
        }

        [DisplayName("Shipment number")]
        public string Number { get; set; }

        public string Status { get; set; }

        [DisplayName("Prenotified")]
        public string SubmittedDate { get; set; }

        [DisplayName("Shipment due")]
        public string ShipmentDate { get; set; }

        [DisplayName("Received")]
        public string ReceivedDate { get; set; }

        public string Quantity { get; set; }

        [DisplayName("Recovered/Disposed")]
        public string CompletedDate { get; set; }

        private string DateValue(DateTime? date, MovementStatus status)
        {
            if (date.HasValue && status != MovementStatus.New && status != MovementStatus.Cancelled)
            {
                return date.Value.ToString("d MMM yyyy");
            }

            if (status == MovementStatus.Cancelled)
            {
                return string.Empty;
            }

            return "- -";
        }

        private string GetStatusDisplay(MovementStatus status, NotificationType type)
        {
            if (status == MovementStatus.Completed)
            {
                return type == NotificationType.Disposal ? "Disposed" : "Recovered";
            }

            return EnumHelper.GetDisplayName(status);
        }
    }
}