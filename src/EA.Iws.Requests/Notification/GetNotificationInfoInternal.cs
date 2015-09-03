namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNotificationInfoInternal : IRequest<NotificationInfo>
    {
        public Guid NotificationId { get; private set; }

        public GetNotificationInfoInternal(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
