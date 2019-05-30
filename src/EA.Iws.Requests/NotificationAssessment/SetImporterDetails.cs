namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Importer;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditContactDetails)]
    public class SetImporterDetails : IRequest<Unit>
    {
        public SetImporterDetails(Guid notificationId, ImporterData importer)
        {
            NotificationId = notificationId;
            Importer = importer;
        }

        public Guid NotificationId { get; private set; }

        public ImporterData Importer { get; private set; }
    }
}