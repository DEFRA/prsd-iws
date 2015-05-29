namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNotificationInfo : IRequest<NotificationInfo>
    {
        public Guid NotificationId { get; set; }

        public GetNotificationInfo(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
