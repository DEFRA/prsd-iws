namespace EA.Iws.Requests.Admin
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditInternalComments)]
    public class CheckNotificationHasComments : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public CheckNotificationHasComments(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
