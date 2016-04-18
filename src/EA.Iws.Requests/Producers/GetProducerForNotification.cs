namespace EA.Iws.Requests.Producers
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Producers;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
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