﻿namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class SetNotificationTransmittedDate : IRequest<bool>
    {
        public SetNotificationTransmittedDate(Guid notificationId, DateTime notificationTransmittedDate)
        {
            NotificationId = notificationId;
            NotificationTransmittedDate = notificationTransmittedDate;
        }

        public Guid NotificationId { get; private set; }

        public DateTime NotificationTransmittedDate { get; private set; }
    }
}