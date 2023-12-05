namespace EA.Iws.Requests.ImportNotification.Producers
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.ImportNotification.Summary;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportContactDetails)]
    public class SetProducerDetailsForImportNotification : IRequest<Unit>
    {
        public SetProducerDetailsForImportNotification(Guid importNotificationId, Producer producerDetails)
        {
            ImportNotificationId = importNotificationId;
            ProducerDetails = producerDetails;
        }

        public Guid ImportNotificationId { get; private set; }

        public Producer ProducerDetails { get; private set; }
    }
}
