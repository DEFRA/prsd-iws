namespace EA.Iws.Requests.Exporters
{
    using System;
    using Core.Authorization;
    using Core.Exporters;
    using Prsd.Core.Mediator;

    [RequestAuthorization("Get Exporter For Export Notification")]
    public class GetExporterByNotificationId : IRequest<ExporterData>
    {
        public Guid NotificationId { get; private set; }

        public GetExporterByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
