namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class ChangeUser : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }
        public Guid NewUserId { get; private set; }

        public ChangeUser(Guid notificationId, Guid newUserId)
        {
            NotificationId = notificationId;
            NewUserId = newUserId;
        }
    }
}
