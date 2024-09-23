namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanChangeNumberOfShipmentsOnExportNotification)]
    public class CreateNotificationStatusChange : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public CreateNotificationStatusChange(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
