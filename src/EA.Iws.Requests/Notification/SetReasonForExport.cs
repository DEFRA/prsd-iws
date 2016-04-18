namespace EA.Iws.Requests.Notification
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetReasonForExport : IRequest<string>
    {
        public SetReasonForExport(Guid notificationId, string reasonForExport)
        {
            NotificationId = notificationId;
            ReasonForExport = reasonForExport;
        }

        public Guid NotificationId { get; private set; }

        public string ReasonForExport { get; private set; }
    }
}