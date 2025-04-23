namespace EA.Iws.Requests.Facilities
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Requests.Authorization;
    using EA.Prsd.Core.Mediator;
    using System;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]

    public class MarkAsInterimToNotification : IRequest<bool>
    {
        public MarkAsInterimToNotification(Guid notificationId, bool isInterim)
        {
            NotificationId = notificationId;
            IsInterim = isInterim;
        }

        public Guid NotificationId { get; private set; }

        public bool IsInterim { get; private set; }
    }
}
