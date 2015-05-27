namespace EA.Iws.Requests.Importer
{
    using System;
    using Prsd.Core.Mediator;

    public class NotificationHasImporter : IRequest<bool>
    {
        public NotificationHasImporter(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}