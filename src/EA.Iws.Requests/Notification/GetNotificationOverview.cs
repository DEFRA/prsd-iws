namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification.Overview;
    using Prsd.Core.Mediator;

    public class GetNotificationOverview : IRequest<NotificationOverview>
    {
        public Guid NotificationId { get; set; }

        public GetNotificationOverview(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
