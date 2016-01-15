namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class ResubmitNotification : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public ResubmitNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}