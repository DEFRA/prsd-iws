namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanDeleteNotification)]
    public class DeleteImportNotification : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public DeleteImportNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
