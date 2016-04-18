namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Prsd.Core.Domain;

    public class NotificationCreatedEvent : IEvent
    {
        public NotificationCreatedEvent(NotificationApplication notification)
        {
            Notification = notification;
        }

        public NotificationApplication Notification { get; private set; }
    }
}