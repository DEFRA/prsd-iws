namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class SetNotificationReceivedDate : IRequest<bool>
    {
        public SetNotificationReceivedDate(Guid notificationId, DateTime notificationReceivedDate)
        {
            NotificationId = notificationId;
            NotificationReceivedDate = notificationReceivedDate;
        }

        public Guid NotificationId { get; private set; }

        public DateTime NotificationReceivedDate { get; private set; }
    }
}