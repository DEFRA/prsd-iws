namespace EA.Iws.Requests.Admin.UserAdministration
{
    using System;
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadUserData)]
    public class GetUserByExportNotificationId : IRequest<ChangeUserData>
    {
        public Guid NotificationId { get; private set; }

        public GetUserByExportNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
