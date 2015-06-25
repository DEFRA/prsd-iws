namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification;
    using Prsd.Core.Mediator;

    public class GetSpecialHandingForNotification : IRequest<SpecialHandlingData>
    {
        public GetSpecialHandingForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}