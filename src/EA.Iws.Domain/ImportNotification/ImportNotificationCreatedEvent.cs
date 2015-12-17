namespace EA.Iws.Domain.ImportNotification
{
    using Prsd.Core.Domain;

    public class ImportNotificationCreatedEvent : IEvent
    {
        public ImportNotification Notification { get; private set; }

        public ImportNotificationCreatedEvent(ImportNotification notification)
        {
            Notification = notification;
        }
    }
}
