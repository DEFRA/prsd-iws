namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core.Domain;
    using System;

    public class SharedUserHistory : Entity
    {
        public Guid NotificationId { get; private set; }

        public string UserId { get; private set; }

        public DateTimeOffset DateAdded { get; private set; }
        public DateTimeOffset DateRemoved { get; private set; }

        protected SharedUserHistory()
        {
        }

        public SharedUserHistory(Guid notificationId, string userId,  DateTimeOffset dateAdded, DateTimeOffset dateRemoved)
        {
            NotificationId = notificationId;
            UserId = userId;
            DateAdded = dateAdded;
            DateRemoved = dateRemoved;
        }
    }
}
