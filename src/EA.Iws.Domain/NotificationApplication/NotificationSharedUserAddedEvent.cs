namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Prsd.Core.Domain;

    public class NotificationSharedUserAddedEvent : IEvent
    {
        public Guid NotificationId { get; private set; }

        public string UserId { get; private set; }

        public NotificationSharedUserAddedEvent(Guid notificationId, string userId)
        {
            NotificationId = notificationId;
            UserId = userId;
        }
    }
}
