namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class SetNotificationAcknowledgedDate : IRequest<bool>
    {
        public SetNotificationAcknowledgedDate(Guid notificationId, DateTime acknowledgedDate)
        {
            NotificationId = notificationId;
            AcknowledgedDate = acknowledgedDate;
        }

        public Guid NotificationId { get; private set; }

        public DateTime AcknowledgedDate { get; private set; }
    }
}