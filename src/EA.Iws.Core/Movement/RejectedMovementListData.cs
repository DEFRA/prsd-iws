namespace EA.Iws.Core.Movement
{
    using System;

    public class RejectedMovementListData
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public DateTime? PrenotificationDate { get; set; }

        public DateTime? ShipmentDate { get; set; }
    }
}
