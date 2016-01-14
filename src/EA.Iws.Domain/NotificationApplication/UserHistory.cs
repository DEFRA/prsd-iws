namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Prsd.Core.Domain;

    public class UserHistory : Entity
    {
        public Guid NotificationId { get; private set; }

        public Guid CurrentUserId { get; private set; }

        public Guid NewUserId { get; private set; }

        public DateTime DateChanged { get; private set; }

        protected UserHistory()
        {
        }

        public UserHistory(Guid notificationId,
            Guid currentUserId,
            Guid newUserId,
            DateTime dateChanged)
        {
            NotificationId = notificationId;
            CurrentUserId = currentUserId;
            NewUserId = newUserId;
            DateChanged = dateChanged;
        }
    }
}
