namespace EA.Iws.Requests.WasteType
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetWasteGenerationProcess : IRequest<Guid>
    {
        public SetWasteGenerationProcess(string process, Guid notificationId, bool isDocumentAttached)
        {
            Process = process;
            NotificationId = notificationId;
            IsDocumentAttached = isDocumentAttached;
        }

        public string Process { get; private set; }
        public Guid NotificationId { get; private set; }
        public bool IsDocumentAttached { get; private set; }
    }
}