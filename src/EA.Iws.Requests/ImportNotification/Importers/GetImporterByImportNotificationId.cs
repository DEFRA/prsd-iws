namespace EA.Iws.Requests.ImportNotification.Importers
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotification.Summary;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class GetImporterByImportNotificationId : IRequest<Importer>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetImporterByImportNotificationId(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
