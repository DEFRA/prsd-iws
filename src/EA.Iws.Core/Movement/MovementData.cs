namespace EA.Iws.Core.Movement
{
    using System;

    [Serializable]
    public class MovementData
    {
        public Guid NotificationId { get; set; }

        public Guid Id { get; set; }

        public int Number { get; set; }
    }
}
