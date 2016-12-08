namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class SetExportNotificationConsultation : IRequest<Guid>
    {
        public SetExportNotificationConsultation(Guid notificationId, Guid localAreaId)
        {
            LocalAreaId = localAreaId;
            NotificationId = notificationId;
        }

        public Guid LocalAreaId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
