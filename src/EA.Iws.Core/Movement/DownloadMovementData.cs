namespace EA.Iws.Core.Movement
{
    using System;
    using System.ComponentModel;
    using Shared;

    public class DownloadMovementData
    {
        [DisplayName("Shipment number")]
        public int Number { get; set; }

        public MovementStatus Status { get; set; }

        [DisplayName("Prenotified")]
        public DateTime? SubmittedDate { get; set; }

        [DisplayName("Shipment due")]
        public DateTime? ShipmentDate { get; set; }

        [DisplayName("Received")]
        public DateTime? ReceivedDate { get; set; }

        public decimal? Quantity { get; set; }

        [DisplayName("Units")]
        public ShipmentQuantityUnits? QuantityUnits { get; set; }

        [DisplayName("Recovered/Disposed")]
        public DateTime? CompletedDate { get; set; }
    }
}
