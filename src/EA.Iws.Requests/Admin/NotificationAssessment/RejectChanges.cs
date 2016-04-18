namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class RejectChanges : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public RejectChanges(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}