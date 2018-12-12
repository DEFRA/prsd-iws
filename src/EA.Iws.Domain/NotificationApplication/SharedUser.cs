namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core.Domain;
    using System;

    public class SharedUser : Entity
    {
        public Guid NotificationId { get; private set; }

        public Guid UserId { get; private set; }

        public DateTimeOffset DateAdded { get; private set; }

        protected SharedUser()
        {
        }

        public SharedUser(Guid notificationId, Guid userId, DateTimeOffset dateAdded)
        {
            NotificationId = notificationId;
            UserId = userId;
            DateAdded = dateAdded;
        }
    }
}
