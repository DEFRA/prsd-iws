namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core.Domain;
    using System;

    public class NotificationSharedUserHistoryAddedEvent : IEvent
    {
        public Guid NotificationId { get; private set; }

        public Guid UserId { get; private set; }

        public DateTimeOffset DateAdded { get; private set; }

        public NotificationSharedUserHistoryAddedEvent(Guid notificationId, Guid userId, DateTimeOffset dateAdded)
        {
            NotificationId = notificationId;
            UserId = userId;
            DateAdded = dateAdded;
        }
    }
}
