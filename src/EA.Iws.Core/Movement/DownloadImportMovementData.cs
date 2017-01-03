namespace EA.Iws.Core.Movement
{
    using System;
    using System.ComponentModel;
    using ImportNotificationMovements;
    using Prsd.Core.Helpers;

    public class DownloadImportMovementData
    {
        [DisplayName("Shipment number")]
        public string Number { get; set; }

        [DisplayName("Prenotified")]
        public string SubmittedDate { get; set; }

        [DisplayName("Shipment due")]
        public string ShipmentDate { get; set; }

        [DisplayName("Received")]
        public string ReceivedDate { get; set; }

        public string Quantity { get; set; }

        [DisplayName("Recovered/Disposed")]
        public string CompletedDate { get; set; }

        public DownloadImportMovementData(MovementTableData data)
        {
            Number = data.Number.ToString();
            SubmittedDate = DateValue(data.PreNotification);
            ShipmentDate = DateValue(data.ShipmentDate, data.IsCancelled);
            ReceivedDate = DateValue(data.Received);
            Quantity = data.Quantity.HasValue ? data.Quantity.Value.ToString("G29") + " " + EnumHelper.GetShortName(data.Unit.GetValueOrDefault()) : "- -";
            CompletedDate = DateValue(data.RecoveredOrDisposedOf);
        }

        private string DateValue(DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToString("d MMM yyyy");
            }

            return "- -";
        }

        private string DateValue(DateTime? date, bool isCancelled)
        {
            if (isCancelled)
            {
                return "Cancelled";
            }

            if (date.HasValue)
            {
                return date.Value.ToString("d MMM yyyy");
            }

            return "- -";
        }
    }
}
