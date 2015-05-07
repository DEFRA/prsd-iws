namespace EA.Iws.Cqrs.ExporterNotifier
{
    using System;
    using Core.Cqrs;
    using Domain;

    public class GetExporterByNotificationId : IQuery<Exporter>
    {
        public Guid NotificationId { get; set; }

        public GetExporterByNotificationId(Guid notificationId)
        {
            this.NotificationId = notificationId;
        }
    }
}
