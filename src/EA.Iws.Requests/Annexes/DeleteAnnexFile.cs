﻿namespace EA.Iws.Requests.Annexes
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class DeleteAnnexFile : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public Guid FileId { get; private set; }

        public DeleteAnnexFile(Guid notificationId, Guid fileId)
        {
            NotificationId = notificationId;
            FileId = fileId;
        }
    }
}
