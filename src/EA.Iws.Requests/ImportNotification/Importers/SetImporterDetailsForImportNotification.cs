namespace EA.Iws.Requests.ImportNotification.Importers
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotification.Summary;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportContactDetails)]
    public class SetImporterDetailsForImportNotification : IRequest<Unit>
    {
        public SetImporterDetailsForImportNotification(Guid importNotificationId, Importer importerDetails)
        {
            ImportNotificationId = importNotificationId;
            ImporterDetails = importerDetails;
        }

        public Guid ImportNotificationId { get; private set; }

        public Importer ImporterDetails { get; private set; }
    }
}
