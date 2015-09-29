namespace EA.Iws.Requests.Producers
{
    using System;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    public class SetSiteOfExport : IRequest<Guid>
    {
        public SetSiteOfExport(Guid producerId, Guid notificationId)
        {
            ProducerId = producerId;
            NotificationId = notificationId;
        }

        public Guid ProducerId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
