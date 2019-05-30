namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Exporters;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditContactDetails)]
    public class SetExporterDetails : IRequest<Unit>
    {
        public SetExporterDetails(Guid notificationId, ExporterData exporter)
        {
            NotificationId = notificationId;
            Exporter = exporter;
        }

        public Guid NotificationId { get; private set; }

        public ExporterData Exporter { get; private set; }
    }
}