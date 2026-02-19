namespace EA.Iws.Requests.NotificationAssessment
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class RemoveUnderProhibitionStatus : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public RemoveUnderProhibitionStatus(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
