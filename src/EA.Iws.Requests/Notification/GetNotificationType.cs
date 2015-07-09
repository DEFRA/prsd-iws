namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNotificationBasicInfo : IRequest<NotificationBasicInfo>
    {
        public GetNotificationBasicInfo(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}