namespace EA.Iws.Requests.ImportNotification.Exporters
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotification.Summary;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportContactDetails)]
    public class SetExporterDetailsForImportNotification : IRequest<Unit>
    {
        public SetExporterDetailsForImportNotification(Guid importNotificationId, Exporter exporterDetails)
        {
            ImportNotificationId = importNotificationId;
            ExporterDetails = exporterDetails;
        }

        public Guid ImportNotificationId { get; private set; }

        public Exporter ExporterDetails { get; private set; }
    }
}
