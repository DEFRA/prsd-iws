namespace EA.Iws.Requests.Producers
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Producers;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetProducersByNotificationId : IRequest<IList<ProducerData>>
    {
        public Guid NotificationId { get; set; }

        public GetProducersByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}