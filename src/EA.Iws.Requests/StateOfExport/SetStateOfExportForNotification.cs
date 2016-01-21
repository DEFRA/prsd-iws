namespace EA.Iws.Requests.StateOfExport
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetStateOfExportForNotification : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }

        public Guid EntryOrExitPointId { get; private set; }

        public SetStateOfExportForNotification(Guid notificationId, Guid entryOrExitPointId)
        {
            NotificationId = notificationId;
            EntryOrExitPointId = entryOrExitPointId;
        }
    }
}
