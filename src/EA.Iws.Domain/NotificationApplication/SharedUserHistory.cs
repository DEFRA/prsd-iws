using EA.Prsd.Core.Domain;
using System;

namespace EA.Iws.Domain.NotificationApplication
{
    public class SharedUserHistory : Entity
    {
        public Guid NotificationId { get; private set; }

        public Guid UserId { get; private set; }

        public DateTimeOffset DateAdded { get; private set; }
        public DateTimeOffset DateRemoved { get; private set; }

        protected SharedUserHistory()
        {
        }

        public SharedUserHistory(Guid notificationId, Guid userId,  DateTimeOffset dateAdded, DateTimeOffset dateRemoved)
        {
            NotificationId = notificationId;
            UserId = userId;
            DateAdded = dateAdded;
            DateRemoved = dateRemoved;
        }
    }
}
