﻿namespace EA.Iws.Requests.WasteType
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetWoodTypeDescription : IRequest<Guid>
    {
        public SetWoodTypeDescription(string woodTypeDescription, Guid notificationId)
        {
            WoodTypeDescription = woodTypeDescription;
            NotificationId = notificationId;
        }

        public string WoodTypeDescription { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}