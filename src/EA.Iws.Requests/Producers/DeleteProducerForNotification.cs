namespace EA.Iws.Requests.Producers
{
    using System;
    using Prsd.Core.Mediator;

    public class DeleteProducerForNotification : IRequest<bool>
    {
        public DeleteProducerForNotification(Guid producerId, Guid notificationId)
        {
            ProducerId = producerId;
            NotificationId = notificationId;
        }

        public Guid ProducerId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}