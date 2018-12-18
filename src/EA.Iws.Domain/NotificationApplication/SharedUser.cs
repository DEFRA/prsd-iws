namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core;
    using Prsd.Core.Domain;
    using System;

    public class SharedUser : Entity
    {
        public Guid NotificationId { get; private set; }

        public string UserId { get; private set; }

        public DateTimeOffset DateAdded { get; private set; }

        protected SharedUser()
        {
        }

        public SharedUser(Guid notificationId, string userId, DateTimeOffset dateAdded)
        {
            Guard.ArgumentNotNullOrEmpty(() => userId, userId);

            NotificationId = notificationId;
            UserId = userId;
            DateAdded = dateAdded;
        }
    }
}
