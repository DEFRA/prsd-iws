namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class DeleteExitCustomsOfficeByNotificationId : IRequest<bool>
    {
        public DeleteExitCustomsOfficeByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}