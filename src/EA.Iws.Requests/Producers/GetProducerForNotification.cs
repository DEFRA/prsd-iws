namespace EA.Iws.Requests.Producers
{
    using System;
    using Prsd.Core.Mediator;

    public class GetProducerForNotification : IRequest<ProducerData>
    {
        public GetProducerForNotification(Guid notificationId, Guid producerId)
        {
            ProducerId = producerId;
            NotificationId = notificationId;
        }

        public Guid ProducerId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}