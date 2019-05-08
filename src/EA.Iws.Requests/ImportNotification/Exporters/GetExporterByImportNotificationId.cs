namespace EA.Iws.Requests.ImportNotification.Exporters
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotification.Summary;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class GetExporterByImportNotificationId : IRequest<Exporter>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetExporterByImportNotificationId(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
