namespace EA.Iws.Requests.Annexes
{
    using System;
    using Core.Annexes;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetAnnexFile : IRequest<AnnexFileData>
    {
        public Guid NotificationId { get; private set; }

        public Guid FileId { get; private set; }

        public GetAnnexFile(Guid notificationId, Guid fileId)
        {
            NotificationId = notificationId;
            FileId = fileId;
        }
    }
}
