namespace EA.Iws.Cqrs.Notification
{
    using System;
    using Core.Cqrs;

    public class GetNotificationNumber : IQuery<string>
    {
        public GetNotificationNumber(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}