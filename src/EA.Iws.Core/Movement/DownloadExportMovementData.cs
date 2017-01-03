namespace EA.Iws.Core.Movement
{
    using System;
    using System.ComponentModel;
    using Prsd.Core.Helpers;

    public class DownloadExportMovementData
    {
        [DisplayName("Shipment number")]
        public string Number { get; set; }

        public MovementStatus Status { get; set; }

        [DisplayName("Prenotified")]
        public string SubmittedDate { get; set; }

        [DisplayName("Shipment due")]
        public string ShipmentDate { get; set; }

        [DisplayName("Received")]
        public string ReceivedDate { get; set; }

        public string Quantity { get; set; }

        [DisplayName("Recovered/Disposed")]
        public string CompletedDate { get; set; }

        public DownloadExportMovementData(MovementTableDataRow data)
        {
            Number = data.Number.ToString();
            Status = data.Status;
            SubmittedDate = DateValue(data.SubmittedDate);
            ShipmentDate = DateValue(data.ShipmentDate);
            ReceivedDate = DateValue(data.ReceivedDate);
            Quantity = data.Quantity.HasValue ? data.Quantity.Value.ToString("G29") + " " + EnumHelper.GetShortName(data.QuantityUnits.GetValueOrDefault()) : "- -";
            CompletedDate = DateValue(data.CompletedDate);
        }

        private string DateValue(DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToString("d MMM yyyy");
            }
            else
            {
                return "- -";
            }
        }
    }
}
