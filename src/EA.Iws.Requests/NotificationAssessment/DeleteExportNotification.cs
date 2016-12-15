namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanDeleteNotification)]
    public class DeleteExportNotification : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public DeleteExportNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
