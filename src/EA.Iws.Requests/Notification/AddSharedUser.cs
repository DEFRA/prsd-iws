namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanChangeExportNotificationOwner)]
    public class AddSharedUser : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }
        public Guid UserId { get; private set; }

        public AddSharedUser(Guid notificationId, Guid userId)
        {
            NotificationId = notificationId;
            UserId = userId;
        }
    }
}
