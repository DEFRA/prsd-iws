namespace EA.Iws.Core.Movement
{
    using System;

    public class SubmittedMovement
    {
        public Guid MovementId { get; set; }

        public Guid NotificationId { get; set; }

        public int Number { get; set; }

        public DateTime? PrenotificationDate { get; set; }

        public DateTime ShipmentDate { get; set; }

        public bool IsSelected { get; set; }
    }
}
