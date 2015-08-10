namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification;
    using Prsd.Core.Mediator;

    public class GetNotificationProgressInfo : IRequest<NotificationApplicationCompletionProgress>
    {
        public GetNotificationProgressInfo(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}