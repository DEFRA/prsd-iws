namespace EA.Iws.Requests.Exporters
{
    using System;
    using Prsd.Core.Mediator;

    public class NotificationHasExporter : IRequest<bool>
    {
        public NotificationHasExporter(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}