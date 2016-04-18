namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class SetExportNotificationConsultation : IRequest<Guid>
    {
        public SetExportNotificationConsultation(Guid notificationId, Guid localAreaId, DateTime? receivedDate)
        {
            LocalAreaId = localAreaId;
            NotificationId = notificationId;
            ReceivedDate = receivedDate;
        }

        public Guid LocalAreaId { get; private set; }

        public Guid NotificationId { get; private set; }

        public DateTime? ReceivedDate { get; private set; }
    }
}
