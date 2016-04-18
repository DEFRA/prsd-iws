namespace EA.Iws.Requests.Importer
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Importer;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetImporterByNotificationId : IRequest<ImporterData>
    {
        public GetImporterByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}