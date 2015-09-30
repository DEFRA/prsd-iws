namespace EA.Iws.Requests.Notification
{
    using System;
    using Overview;
    using Prsd.Core.Mediator;

    public class GetNotificationInfoInternal : IRequest<NotificationOverview>
    {
        public Guid NotificationId { get; private set; }

        public GetNotificationInfoInternal(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
