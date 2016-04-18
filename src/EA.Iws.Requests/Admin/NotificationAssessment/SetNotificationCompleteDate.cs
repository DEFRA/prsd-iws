namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class SetNotificationCompleteDate : IRequest<bool>
    {
        public SetNotificationCompleteDate(Guid notificationId, DateTime notificationCompleteDate)
        {
            NotificationId = notificationId;
            NotificationCompleteDate = notificationCompleteDate;
        }

        public Guid NotificationId { get; private set; }

        public DateTime NotificationCompleteDate { get; private set; }
    }
}