﻿namespace EA.Iws.Requests.Producers
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
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