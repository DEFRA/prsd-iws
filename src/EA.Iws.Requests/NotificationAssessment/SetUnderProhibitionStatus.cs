namespace EA.Iws.Requests.NotificationAssessment
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class SetUnderProhibitionStatus : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public SetUnderProhibitionStatus(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
