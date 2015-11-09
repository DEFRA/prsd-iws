namespace EA.Iws.Core.Movement
{
    using System;
    using Shared;

    public class MovementTableDataRow
    {
        public int Number { get; set; }

        public MovementStatus Status { get; set; }

        public DateTime? SubmittedDate { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? QuantityUnits { get; set; }

        public DateTime? CompletedDate { get; set; }
    }
}
