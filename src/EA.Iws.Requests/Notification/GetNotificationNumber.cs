namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNotificationNumber : IRequest<string>
    {
        public Guid NotificationId { get; private set; }

        public GetNotificationNumber(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
