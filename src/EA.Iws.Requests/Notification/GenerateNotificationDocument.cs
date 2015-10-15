namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Documents;
    using Prsd.Core.Mediator;

    public class GenerateNotificationDocument : IRequest<FileData>
    {
        public GenerateNotificationDocument(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}