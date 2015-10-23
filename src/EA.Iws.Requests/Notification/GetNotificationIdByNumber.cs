namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNotificationIdByNumber : IRequest<Guid?>
    {
        public string NotificationNumber { get; private set; }

        public GetNotificationIdByNumber(string notificationNumber)
        {
            NotificationNumber = notificationNumber;
        }
    }
}