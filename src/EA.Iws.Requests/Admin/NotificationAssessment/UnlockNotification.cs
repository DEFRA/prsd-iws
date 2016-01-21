namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class UnlockNotification : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public UnlockNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}