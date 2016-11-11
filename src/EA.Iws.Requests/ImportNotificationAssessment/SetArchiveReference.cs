namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotificationAssessment)]
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