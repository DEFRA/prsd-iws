namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Prsd.Core.Domain;

    public class NotificationUserChangedEvent : IEvent
    {
        public Guid NotificationId { get; private set; }

        public Guid CurrentUserId { get; private set; }

        public Guid NewUserId { get; private set; }

        public NotificationUserChangedEvent(Guid notificationId,
            Guid currentUserId,
            Guid newUserId)
        {
            NotificationId = notificationId;
            CurrentUserId = currentUserId;
            NewUserId = newUserId;
        }
    }
}
