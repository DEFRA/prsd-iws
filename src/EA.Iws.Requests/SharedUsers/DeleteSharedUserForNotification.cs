namespace EA.Iws.Requests.SharedUsers
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using System;
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class DeleteSharedUserForNotification : IRequest<bool>
    {
        public DeleteSharedUserForNotification(Guid notificationId, Guid sharedId)
        {
            NotificationId = notificationId;
            SharedId = sharedId;
        }

        public Guid SharedId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
