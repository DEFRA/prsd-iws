namespace EA.Iws.Requests.SharedUsers
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetSharedUserById : IRequest<NotificationSharedUser>
    {
        public GetSharedUserById(Guid notificationId, Guid sharedId)
        {
            SharedId = sharedId;
            NotificationId = notificationId;
        }

        public Guid SharedId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
