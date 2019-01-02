namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using System.Collections.Generic;

    [RequestAuthorization(ExportNotificationPermissions.CanChangeExportNotificationOwner)]
    public class AddSharedUser : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }
        public string UserId { get; private set; }

        public List<string> UserIds { get; private set; }

        public AddSharedUser(Guid notificationId, List<string> userIds)
        {
            NotificationId = notificationId;
            UserIds = userIds;
        }
    }
}
