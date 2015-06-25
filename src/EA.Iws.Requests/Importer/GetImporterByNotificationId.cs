namespace EA.Iws.Requests.Importer
{
    using System;
    using Core.Importer;
    using Prsd.Core.Mediator;

    public class GetImporterByNotificationId : IRequest<ImporterData>
    {
        public GetImporterByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}