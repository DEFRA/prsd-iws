namespace EA.Iws.Requests.SharedUsers
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetSharedUsersByNotificationId : IRequest<IList<NotificationSharedUser>>
    {
        public Guid NotificationId { get; private set; }

        public GetSharedUsersByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
