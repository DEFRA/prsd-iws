namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification.Overview;
    using Prsd.Core.Mediator;

    public class GetNotificationOverviewInternal : IRequest<NotificationOverview>
    {
        public Guid NotificationId { get; private set; }

        public GetNotificationOverviewInternal(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
