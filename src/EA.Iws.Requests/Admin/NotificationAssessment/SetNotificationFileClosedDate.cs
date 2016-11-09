namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class SetNotifcationFileClosedDate : IRequest<Unit>
    {
        public SetNotifcationFileClosedDate(Guid notificationId, DateTime fileClosedDate)
        {
            NotificationId = notificationId;
            FileClosedDate = fileClosedDate;
        }

        public Guid NotificationId { get; private set; }

        public DateTime FileClosedDate { get; private set; }
    }
}