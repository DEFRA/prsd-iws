namespace EA.Iws.Requests.DeleteNotification
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ExportNotificationPermissions.CanExternalUserDeleteNotification)]
    public class DeleteExportNotification : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public DeleteExportNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
