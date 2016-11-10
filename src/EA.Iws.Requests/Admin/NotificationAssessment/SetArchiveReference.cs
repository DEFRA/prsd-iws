namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class SetArchiveReference : IRequest<Unit>
    {
        public SetArchiveReference(Guid notificationId, string archiveReference)
        {
            NotificationId = notificationId;
            ArchiveReference = archiveReference;
        }

        public Guid NotificationId { get; private set; }

        public string ArchiveReference { get; private set; }
    }
}