namespace EA.Iws.Requests.SharedUsers
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using System;
    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class CheckIfSharedUserExists : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public CheckIfSharedUserExists(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
