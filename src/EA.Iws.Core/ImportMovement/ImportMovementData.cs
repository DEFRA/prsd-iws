namespace EA.Iws.Core.ImportMovement
{
    using System;
    using Shared;

    public class ImportMovementData
    {
        public NotificationType NotificationType { get; set; }

        public Guid NotificationId { get; set; }

        public int Number { get; set; }

        public DateTimeOffset ActualDate { get; set; }

        public DateTimeOffset? PreNotificationDate { get; set; }

        public bool IsCancelled { get; set; }
    }
}
