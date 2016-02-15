namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class SetExportNotificationLocalAreaId : IRequest<Guid>
    {
        public SetExportNotificationLocalAreaId(Guid notificationId, Guid localAreaId)
        {
            LocalAreaId = localAreaId;
            NotificationId = notificationId;
        }

        public Guid LocalAreaId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
