namespace EA.Iws.Requests.Producers
{
    using System;
    using Prsd.Core.Mediator;

    public class CopyProducerFromExporter : IRequest<Guid>
    {
        public CopyProducerFromExporter(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}