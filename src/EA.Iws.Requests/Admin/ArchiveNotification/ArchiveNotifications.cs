namespace EA.Iws.Requests.Admin.ArchiveNotification
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.Admin.ArchiveNotification;
    using Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;

    [RequestAuthorization(GeneralPermissions.CanViewSearchResults)]
    public class ArchiveNotifications : IRequest<IList<ArchiveNotificationResult>>
    {
        public List<Guid> NotificationIds { get; private set; }

        public ArchiveNotifications(List<Guid> notificationIds)
        {
            NotificationIds = notificationIds;
        }
    }
}