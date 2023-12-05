namespace EA.Iws.Requests.ImportNotification.Producers
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotification.Summary;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class GetProducerByImportNotificationId : IRequest<Producer>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetProducerByImportNotificationId(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
