namespace EA.Iws.Core.Movement
{
    using System;

    [Serializable]
    public class AddedCancellableMovement
    {
        public Guid NotificationId { get; set; }

        public int Number { get; set; }

        public DateTime ShipmentDate { get; set; }
    }
}
