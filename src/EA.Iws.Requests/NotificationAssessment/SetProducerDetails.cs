namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.Producers;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditContactDetails)]
    public class SetProducerDetails : IRequest<Unit>
    {
        public SetProducerDetails(Guid notificationId, ProducerData producer)
        {
            NotificationId = notificationId;
            Producer = producer;
        }

        public Guid NotificationId { get; private set; }

        public ProducerData Producer { get; private set; }
    }
}
