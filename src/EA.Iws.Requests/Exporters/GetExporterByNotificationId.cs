namespace EA.Iws.Requests.Exporters
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Exporters;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetExporterByNotificationId : IRequest<ExporterData>
    {
        public Guid NotificationId { get; private set; }

        public GetExporterByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
