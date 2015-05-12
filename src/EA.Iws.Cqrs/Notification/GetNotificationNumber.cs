namespace EA.Iws.Cqrs.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNotificationNumber : IRequest<string>
    {
        public GetNotificationNumber(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}