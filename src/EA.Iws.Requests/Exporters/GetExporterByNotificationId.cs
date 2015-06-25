namespace EA.Iws.Requests.Exporters
{
    using System;
    using Core.Exporters;
    using Prsd.Core.Mediator;

    public class GetExporterByNotificationId : IRequest<ExporterData>
    {
        public Guid NotificationId { get; private set; }

        public GetExporterByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
