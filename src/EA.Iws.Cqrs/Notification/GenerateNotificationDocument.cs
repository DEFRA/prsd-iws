namespace EA.Iws.Cqrs.Notification
{
    using System;
    using Core.Cqrs;

    public class GenerateNotificationDocument : IQuery<byte[]>
    {
        public Guid NotificationId { get; private set; }

        public string ApplicationDirectory { get; private set; }

        public GenerateNotificationDocument(Guid notificationId, string applicationDirectory)
        {
            NotificationId = notificationId;
            ApplicationDirectory = applicationDirectory;
        }
    }
}
