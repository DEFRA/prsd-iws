namespace EA.Iws.Requests.Files
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Files;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetFile : IRequest<FileData>
    {
        public Guid NotificationId { get; private set; }

        public Guid FileId { get; private set; }

        public GetFile(Guid notificationId, Guid fileId)
        {
            NotificationId = notificationId;
            FileId = fileId;
        }
    }
}
